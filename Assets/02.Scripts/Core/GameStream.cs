using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using Random = UnityEngine.Random;

public class GameStream : MonoBehaviour, IRefreshable
{
    public event Action OnChanged;

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private BoardController _board;
    [SerializeField] private BattlePanel _battlePanel;

    private bool _isRoll = false;
    private bool _isClose = false;
    private bool _isSelected = false;
    private Coroutine _streamCo;
    private Player _player;
    private Enemy _enemy;
    private int _round = 1;
    private Tile _selectedTile;

    public Player Player => _player;
    public Enemy Enemy => _enemy;
    public int Round => _round;

    private void Start()
    {
        _streamCo = StartCoroutine(StreamCo());
    }

    public void RollDice()
    {
        _isRoll = true;
    }
    public void CloseEvent()
    {
        _isClose = true;
    }

    private IEnumerator StreamCo()
    {
        yield return null;

        // 게임 데이터 초기화
        // 타일 변경
        _board.DrawTiles();
        
        // 플레이어 생성
        _player = Instantiate(_playerPrefab);
        _player.SpawnToTile(_board.Tiles[0]);

        // 적 생성
        _enemy = Instantiate(_enemyPrefab);
        _enemy.SpawnToTile(_board.Tiles[_board.Tiles.Count - 1]);

        OnChanged?.Invoke();

        // 게임 루프 시작
        while (true)
        {
            // 플레이어 턴
            yield return StartCoroutine(PlayerTurnCo());

            // 적 턴
            yield return StartCoroutine(EnemyTurnCo());

            // 라운드 진행
            _round++;
            OnChanged?.Invoke();
        }

        // 
    }
    private IEnumerator PlayerTurnCo()
    {
        // 효과 카드 사용 가능

        // 주사위 굴리기
        // 임시로직으로 바로 랜덤 값 생성(추후 주사위 굴리는 이벤트로 수정예정)
        yield return new WaitUntil(() => _isRoll);
        _isRoll = false;
        int rand = Random.Range(1, 11);
        Debug.Log($"플레이어 주사위 값 : {rand}");

        // 칸 만큼 이동
        for (int i = 0; i < rand; i++)
        {
            Tile currentTile = _player.CurrentTile;
            Tile nextTile = null;
            List<Tile> nextTiles = new List<Tile>();

            foreach (var next in currentTile.NextTiles)
            {
                // 플레이어가 직전에 밟았던 타일인 경우 건너뛰기
                if (_player.PrevTile != null && next == _player.PrevTile)
                {
                    continue;
                }
                nextTiles.Add(next);
            }

            // 다음 타일을 못찾음
            if (nextTiles.Count <= 0)
            {
                Debug.LogWarning("nextTile을 찾지 못함");
            }
            // 분기점이 없는 경우
            if (nextTiles.Count == 1)
            {
                nextTile = nextTiles[0];
            }
            // 분기점이 있는 경우 이동 방향 선택
            else
            {
                Debug.Log("분기점 도착");
                foreach (Tile tile in nextTiles)
                {
                    tile.SetArrow(true);
                }
                // 이동 방향 선택 대기
                yield return new WaitUntil(() =>
                {
                    if (!Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        return false;
                    }
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    var hit = Physics2D.Raycast(mousePos, Vector2.zero);
                    if (hit.collider != null && hit.collider.TryGetComponent(out _selectedTile))
                    {
                        return _selectedTile.Selectable;
                    }
                    else
                    {
                        return false;
                    }
                });
                _isSelected = false;
                nextTile = _selectedTile;

                foreach (Tile tile in nextTiles)
                {
                    tile.SetArrow(false);
                }
            }

            // 플레이어 다음 타일로 이동
            _player.MoveToTile(nextTile);

            // 일정 초 기다리기(순간이동에서 이동으로 구현되면 초에서 코루틴으로 변경될 예정)
            yield return new WaitForSeconds(0.1f);

            // 현재 타일에 적이 있는지 확인
            if (_player.CurrentTile == _enemy.CurrentTile)
            {
                Debug.Log("플레이어가 적과 접촉");
                yield return StartCoroutine(PlayerAttackPhase());
                // 전투 후 전투 결과 확인
            }

            // 마지막 무브 이거나 이동 중간에 발생하는 이벤트인 경우
            if (i == rand - 1 || IsMoveActivate(_player.CurrentTile.Type))
            {
                Debug.Log($"{_player.CurrentTile.Type} 이벤트 발생");
                //yield return new WaitUntil(() => _isClose);
            }
        }
    }
    private IEnumerator EnemyTurnCo()
    {
        yield return null;
        int rand = Random.Range(1, 11);
        Debug.Log($"적 주사위 값 : {rand}");

        // 칸 만큼 이동
        for (int i = 0; i < rand; i++)
        {
            Tile currentTile = _enemy.CurrentTile;
            Tile nextTile = null;
            List<Tile> nextTiles = new List<Tile>();

            // 분기점이 없는 경우
            foreach (var next in currentTile.NextTiles)
            {
                // 적이 직전에 밟았던 타일인 경우 건너뛰기
                if (_enemy.PrevTile != null && next == _enemy.PrevTile)
                {
                    continue;
                }
                //nextTile = next;
                nextTiles.Add(next);
            }

            // 다음 타일을 못찾음
            if (nextTiles.Count <= 0)
            {
                Debug.LogWarning("nextTile을 찾지 못함");
            }
            // 분기점이 없는 경우
            if (nextTiles.Count == 1)
            {
                nextTile = nextTiles[0];
            }
            // 분기점이 있는 경우 플레이어에 더 가까운 방향으로 이동
            else
            {
                EnemySelectDirection(nextTiles, currentTile, out nextTile);
            }

            // 플레이어 다음 타일로 이동
            _enemy.MoveToTile(nextTile);

            // 일정 초 기다리기(순간이동에서 이동으로 구현되면 초에서 코루틴으로 변경될 예정)
            yield return new WaitForSeconds(0.1f);

            // 현재 타일에 플레이어가 있는지 확인
            if (_enemy.CurrentTile == _player.CurrentTile)
            {
                Debug.Log("적이 플레이어와 접촉");
                yield return StartCoroutine(EnemyAttackPhase());
                // 전투 후 전투 결과 확인
            }
        }
    }
    private IEnumerator PlayerAttackPhase()
    {
        // 전투화면 UI 갱신
        _battlePanel.DrawInfo(_player, _enemy);
        _battlePanel.DrawDiceValue(0, 0);
        _battlePanel.gameObject.SetActive(true);
        // 카드 사용 등등의 행동 후
        // 주사위 굴리기
        yield return new WaitUntil(() => _isRoll);
        _isRoll = false;

        // 플레이어 주사위 눈 결과
        int playerDice = Random.Range(1, 7);
        // 플레이어 최종 공격력
        int playerDamage = _player.AttackDamage + playerDice;
        Debug.Log($"플레이어 눈 결과 : {playerDice}, 최종 공격력 : {playerDamage}");
        _battlePanel.DrawDiceValue(playerDice, 0);

        yield return new WaitForSeconds(0.5f);

        // 적 주사위 눈 결과
        int enemyDice = Random.Range(1, 7);
        // 적 최종 방어력
        int enemyDefense = _enemy.Defense + enemyDice;
        Debug.Log($"적 눈 결과 : {enemyDice}, 최종 방어력 : {enemyDefense}");
        _battlePanel.DrawDiceValue(playerDice, enemyDice);
        yield return new WaitForSeconds(0.5f);

        _battlePanel.DrawDiceValue(playerDamage, enemyDefense);
        yield return new WaitForSeconds(0.5f);

        // 최종 데미지 만큼 공격
        int resultDamage = Mathf.Max(1, playerDamage - enemyDefense);
        Debug.Log($"최종 데미지 : {resultDamage}");
        _enemy.TakeDamage(resultDamage);

        _battlePanel.DrawInfo(_player, _enemy);
        yield return new WaitForSeconds(0.5f);

        _battlePanel.gameObject.SetActive(false);
    }
    private IEnumerator EnemyAttackPhase()
    {
        // 전투화면 UI 갱신로직 연결 추가예정
        _battlePanel.DrawInfo(_enemy, _player);
        _battlePanel.DrawDiceValue(0, 0);
        _battlePanel.gameObject.SetActive(true);

        // 카드 기능 추가되면 카드 사용 Flow 추가 예정

        // 적 주사위 눈 결과
        int enemyDice = Random.Range(1, 7);
        // 적 최종 공격력
        int enemyDamage = _enemy.AttackDamage + enemyDice;
        Debug.Log($"적 눈 결과 : {enemyDice}, 최종 공격력 : {enemyDamage}");
        _battlePanel.DrawDiceValue(enemyDice, 0);

        // 카드 사용 등등의 행동 후
        // 주사위 굴리기
        yield return new WaitUntil(() => _isRoll);
        _isRoll = false;

        // 회피 분기 추가 예정
        
        // 플레이어 주사위 눈 결과
        int playerDice = Random.Range(1, 7);
        // 플레이어 최종 공격력
        int playerDefense = _player.Defense + playerDice;
        Debug.Log($"플레이어 눈 결과 : {playerDice}, 최종 방어력 : {playerDefense}");
        _battlePanel.DrawDiceValue(enemyDice, playerDice);
        yield return new WaitForSeconds(0.5f);

        _battlePanel.DrawDiceValue(enemyDamage, playerDefense);
        yield return new WaitForSeconds(0.5f);

        // 최종 데미지 만큼 피격
        int resultDamage = Mathf.Max(1, enemyDamage - playerDefense);
        Debug.Log($"최종 데미지 : {resultDamage}");
        _player.TakeDamage(resultDamage);

        _battlePanel.DrawInfo(_enemy, _player);
        yield return new WaitForSeconds(0.5f);

        _battlePanel.gameObject.SetActive(false);
    }

    private void EnemySelectDirection(List<Tile> tiles, Tile prev, out Tile select)
    {
        Queue<(Tile tile, Tile prev, Tile root)> queue = new();
        HashSet<Tile> visited = new HashSet<Tile>();
        foreach (Tile tile in tiles)
        {
            queue.Enqueue((tile, prev, tile));
            visited.Add(tile);
        }
        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            if (cur.tile == _player.CurrentTile)
            {
                select = cur.root;
                return;
            }
            foreach (Tile next in cur.tile.NextTiles)
            {
                if (next == cur.prev || visited.Contains(next))
                {
                    continue;
                }
                queue.Enqueue((next, cur.tile, cur.root));
                visited.Add(next);
            }
        }
        select = tiles[0];
    }
    // 이동 중 발생하는 이벤트인지 여부를 반환하는 메서드
    private bool IsMoveActivate(ETileType type)
    {
        return type == ETileType.LevelUp || type == ETileType.CardShop || type == ETileType.ChipShop;
    }
}
