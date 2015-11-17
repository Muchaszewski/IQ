using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public int MaxDots = 3;
    public float Speed = 1;

    private Text _text;
    private float _timer;
    private int _dots;

    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = "Loading";
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > Speed)
        {
            var text = "Loading";
            _dots++;
            if (_dots == MaxDots + 1)
            {
                _dots = 0;
            }
            for (int i = 0; i < _dots; i++)
            {
                text += ".";
            }
            _text.text = text;
            _timer = 0;
        }
    }
}
