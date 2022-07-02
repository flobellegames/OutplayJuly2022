using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionThreeText : MonoBehaviour
{
	[SerializeField] RectTransform _rectTransform;
    [SerializeField] Text _text;

    public void Setup(int t, float x, float y)
	{
		_rectTransform.anchoredPosition = new Vector2(x, y);
		_text.text = t.ToString();
	}

}
