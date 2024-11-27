using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate () {
            OnClick(param);
        });
    }
}

public class manager : MonoBehaviour
{
    private List<Sprite> fill  = new List<Sprite>();
    private List<Sprite> empty = new List<Sprite>();
    private Dictionary<int,int> key = new Dictionary<int,int>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject parent_empty;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] protected TextMeshProUGUI result;
    [SerializeField] private GameObject retry;
    int selected = -1;
    float time = 60;
    int captured = 0;
    void Start()
    {
        parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width/ 5, (Screen.height / 2) / 5);
        parent_empty.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width / 5, ((Screen.height - (timer.GetComponent<RectTransform>().sizeDelta.y+((-1* timer.GetComponent<RectTransform>().anchoredPosition.y)))) / 2) / 5);
        fill = Resources.LoadAll<Sprite>("fill").ToList();
        empty = Resources.LoadAll<Sprite>("empty").ToList();
        Shuffle(fill);
        for (int i = 0; i < fill.Count; i++)
        {
            GameObject g = Instantiate(prefab,parent.transform);
            g.GetComponent<Image>().sprite = fill[i];
            g.GetComponent<Button>().AddEventListener(i,Click);
        }
        for (int i = 0;i < empty.Count; i++)
        {
            GameObject g = Instantiate(prefab, parent_empty.transform);
            g.GetComponent<Image>().sprite = empty[i];
            g.GetComponent<Button>().AddEventListener(i, Click_Iteam);
        }
    }
    private void Click(int i)
    {
        selected = i;
    }
    private void Click_Iteam(int i)
    {
        if (key[selected] == i)
        {
            parent_empty.transform.GetChild(i);
            Sprite temp = parent.transform.GetChild(selected).GetComponent<Image>().sprite;
            parent.transform.GetChild(selected).GetComponent<Image>().sprite = parent_empty.transform.GetChild(i).GetComponent<Image>().sprite;
            parent_empty.transform.GetChild(i).GetComponent<Image>().sprite=temp;
            captured++;
            counter.text = captured.ToString();
        }
        if(captured==25)
        {
            result.color = Color.green;
            result.text = "You won.";
            retry.SetActive(true);
        }
    }
    void Update()
    {
        if (time <= 0 && result.text != "You won.")
        {
            result.color = Color.red;
            result.text = "You lost.";
            retry.SetActive(true);
        }
        else
        {
            time -= Time.deltaTime;
            timer.text = MathF.Round(time).ToString();
        }
    }
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            key[i] = i;
        }

        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int tempIndex = key[i];
            key[i] = key[j];
            key[j] = tempIndex;
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
    public void Retry()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
