using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class GetStatAdvance : MonoBehaviour
{

    [HideInInspector]
    [Tooltip("Merge few stats into one stat")]
    public GetStat[] Stats;

    [Tooltip("String that will appear between every stat added eg. space or sign")]
    public string MergeString;

    private Text _textComponent;

    // Use this for initialization
    void Start()
    {
        _textComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_EDITOR
    public string SetPreview()
    {
        StringBuilder builder = new StringBuilder();
        for (int index = 0; index < Stats.Length; index++)
        {
            var stat = Stats[index];
            builder.Append(stat.SetPreview());
            if (index < Stats.Length - 1)
            {
                builder.Append(MergeString);
            }
        }
        return builder.ToString();
    }
#endif
}
