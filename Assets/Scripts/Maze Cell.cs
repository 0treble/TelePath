using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        if (_unvisitedBlock != null)
        {
            _unvisitedBlock.SetActive(false);
        }
    }

    public void Reset()
    {
        IsVisited = false;
        if (_unvisitedBlock != null)
        {
            _unvisitedBlock.SetActive(true);
        }
        _leftWall.SetActive(true);
        _rightWall.SetActive(true);
        _frontWall.SetActive(true);
        _backWall.SetActive(true);
    }

    public void ClearLeftWall()
    {
        if (_leftWall != null) _leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        if (_rightWall != null) _rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        if (_frontWall != null) _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        if (_backWall != null) _backWall.SetActive(false);
    }
}
