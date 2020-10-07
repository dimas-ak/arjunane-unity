/*
    Developed by : Dimas Awang Kusuma
    Documentation
        -> Online : https://dimas-ak.web.app/documentation/arjunane-unity
        -> Offline: index.html
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ArjunaneLibrary
{
    public class Arjunane
    {
        private ServerExecute server = null;
        // Use this for initialization
        private string isServerDone = "";
        private string _elem, _url, _errorHTTP, _textHTTP;
        public int Index { get { return GetIndex.Index; } }
        private int _length { get; set; }
        // public int _Length { get { return this._length; } }
        public int Length { get { return this._length; } }
        public string version = "1.1.0 [Arjunane]";
        private bool _resServer;
        private Dictionary<string, string> _field = new Dictionary<string, string>();
        private GameObject _GameObject;
        private GameObject[] _GameObjects;
        public GameObject GameObject
        {
            get { return _GameObject; }
        }
        public GameObject[] GameObjects 
        {
            get { return _GameObjects; }
        }
        public Dropdown Dropwdown;
        /*
			GAMEOBJECT
		*/
        //get name element GameObject
        public Arjunane Get(params string[] elem)
        {
            var ar = new Arjunane();

            ar.InitElem(null, null, elem);
            return ar;
        }
        public Arjunane Get(GameObject go)
        {
            var ar = new Arjunane();

            ar.InitElem(go);
            return ar;
        }
        public Arjunane Get(GameObject[] go)
        {
            var ar = new Arjunane();

            ar.InitElem(null, go);
            return ar;
        }
        private Arjunane InitElem(GameObject go = null, GameObject[] gos = null, params string[] names)
        {
            List<GameObject> list = new List<GameObject>();
            if (go != null)
            {
                list.Add(go);
            }
            else if (gos != null)
            {
                foreach (var ini in gos) list.Add(ini);
            }
            else
            {
                List<string> NotInArray = new List<string>();
                foreach (GameObject gom in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    string name = gom.transform.name.ToString();
                    if (names.Contains(name))
                    {
                        list.Add(gom);
                        if (!NotInArray.Contains(name)) NotInArray.Add(name);
                    }
                }
                foreach (var name in names)
                {
                    if (!NotInArray.Contains(name))
                    {
                        _elem = name;
                        throwError();
                    }
                }
            }

            var _ini = NormalizeSortGameObject(list.ToArray());

            _GameObject = _ini[0];
            _GameObjects = _ini.ToArray();
            // GetLength.Length = _ini.Length;
            _length = _ini.Length;
            return this;
        }
        private GameObject[] NormalizeSortGameObject(GameObject[] go)
        {
            List<GetList> newList = new List<GetList>();
            for (int i = 0; i < go.Length; i++)
            {
                GameObject ini_go = go[i];
                int sibling = ini_go.transform.GetSiblingIndex();
                newList.Add(new GetList { gameObject = ini_go, sibling = sibling });
            }

            // normalisasi pengurutan berdasarkan urutan pada hierarchy
            newList.Sort(delegate (GetList x, GetList y)
            {
                return x.sibling.CompareTo(y.sibling);
            });

            List<GameObject> setList = new List<GameObject>();

            for (int i = 0; i < newList.Count; i++)
            {
                var ini_list = newList[i];
                GameObject ini_go = ini_list.gameObject;
                setList.Add(ini_go);
            }
            GameObject[] arr = setList.ToArray();
            return arr;
        }
        public void InsertGameObject(GameObject gameobject, Vector3 vector3, Quaternion quaternion)
        {
            GameObject instantiate = UnityEngine.Object.Instantiate(gameobject, vector3, quaternion);
            instantiate.name = instantiate.name.Replace("(Clone)", "");
        }
        public void Clone(GameObject parent, GameObject child, Action<GameObject> act = null, int how_many = 1, bool world_position = false)
        {
            for (int i = 0; i < how_many; i++)
            {
                GameObject instantiate = UnityEngine.Object.Instantiate(child);
                instantiate.name = instantiate.name.Replace("(Clone)", "");
                act?.Invoke(instantiate);
                instantiate.transform.SetParent(parent.transform, world_position);
            }
        }
        public Arjunane Find(params string[] names)
        {
            var parents = _GameObjects;
            var ar = new Arjunane();
            ar.InitElemFind(null, parents, names);
            return ar;
        }
        public Arjunane Find(GameObject parent, params string[] names)
        {
            var ar = new Arjunane();
            ar.InitElemFind(parent, null, names);
            return ar;
        }
        private Arjunane InitElemFind(GameObject parent = null, GameObject[] parents = null, params string[] names)
        {
            List<GameObject> list = new List<GameObject>();
            if (parent == null)
            {
                foreach (var _ini in parents)
                {
                    var find = IsFind(_ini, names);
                    if (find.GameObjects.Count > 0)
                    {
                        foreach (var i in find.GameObjects)
                        {
                            list.Add(i);
                        }
                    }
                    ShowErrorElem(find.NotInArray.ToArray(), names);
                }
            }
            else
            {
                var find = IsFind(parent, names);

                if (find.GameObjects.Count > 0) list = find.GameObjects;

                ShowErrorElem(find.NotInArray.ToArray(), names);
            }

            var ini = NormalizeSortGameObject(list.ToArray());

            _GameObject = ini[0];
            _GameObjects = ini;
            //GetLength.Length = ini.Length;
            _length = ini.Length;

            return this;
        }
        private void ShowErrorElem(string[] NotInArray, params string[] names)
        {
            foreach (var name in names)
            {
                if (!NotInArray.Contains(name))
                {
                    _elem = name;
                    throwError();
                }
            }
        }
        private GetElemFind IsFind(GameObject go, params string[] names)
        {
            GetElemFind gef = new GetElemFind();
            var children = go.GetComponentsInChildren<Transform>(true);
            foreach (var ini in children)
            {
                var name = ini.transform.name;
                if (names.Contains(name))
                {
                    gef.GameObjects.Add(ini.gameObject);
                    if(!gef.NotInArray.Contains(name))gef.NotInArray.Add(name);
                }
            }
            return gef;
        }

        public Arjunane IndexOf(int index)
        {
            GetIndex.Index = index;
            List<GameObject> list = new List<GameObject>
            {
                _GameObjects[index]
            };

            var ar = new Arjunane();
            ar.InitElem(null, list.ToArray());
            return ar;
        }
        public T GetComponent<T>() { return _GameObject.GetComponent<T>(); }
        public T[] GetComponents<T>() { return _GameObject.GetComponents<T>(); }

        private GameObject callBack_ini(int index = -1)
        {
            return index == -1 ? _GameObjects[0] : _GameObjects[index];
        }
        public void ForEach(Action<GameObject, int> callback)
        {
            for (int i = 0; i < _GameObjects.Length; i++)
            {
                GameObject get = _GameObjects[i];
                callback(get, i);
            }
        }
        public Animator Animator()
        {
            GameObject _ini = callBack_ini();
            Animator tex = _ini.GetComponent<Animator>();
            return tex;
        }
        public RawImage RawImage()
        {
            GameObject _ini = callBack_ini();
            RawImage ri = _ini.GetComponent<RawImage>();
            return ri;
        }
        public InputField InputField()
        {
            GameObject _ini = callBack_ini();
            InputField tex = _ini.GetComponent<InputField>();
            return tex;
        }
        public Texture Texture()
        {
            GameObject _ini = callBack_ini();
            Texture tex = _ini.GetComponent<Renderer>().material.GetTexture("_MainTex");
            return tex;
        }
        public Text Text()
        {
            GameObject go = callBack_ini();
            Text text = go.GetComponent<Text>();
            return text;
        }
        public Texture[] Textures()
        {
            List<Texture> list = new List<Texture>();
            for (int i = 0; i < _GameObjects.Length; i++)
            {
                Texture text = _GameObjects[i].GetComponent<Texture>();
                list.Add(text);
            }
            return list.ToArray();
        }
        public Text[] Texts()
        {
            List<Text> list = new List<Text>();
            for (int i = 0; i < _GameObjects.Length; i++)
            {
                Text text = _GameObjects[i].GetComponent<Text>();
                list.Add(text);
            }
            return list.ToArray();
        }
        public void SetText(string text)
        {
            foreach(var ini in _GameObjects)
            {
                ini.GetComponent<Text>().text = text;
            }
        }
        public Color ColorFromHex(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color color);
            return color;
        }
        public void SetMaterial(GameObject go, string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color col))
            {
                go.GetComponent<Renderer>().material.color = col;
            }
        }
        public void SetTexture(GameObject go, Texture tex)
        {
            go.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
        }
        public void SetActive(bool is_active)
        {
            foreach(var ini in _GameObjects)
            {
                ini.SetActive(is_active);
            }
        }
        private void throwError(int name = 0)
        {
            if (name == 0) Debug.LogError("Ops, Element with name " + _elem + " doesn't Exist.");
            else if (name == 1) Debug.LogError("Please add Function for action click.");
            else if (name == 2) Debug.LogError("GameObject is not Button.");
        }
        public void Click(UnityAction method)
        {
            // if element Untagged or something is exist
            if (_GameObjects.Length != 0)
            {
                // check if action method are not null
                if (method != null)
                {
                    for (int i = 0; i < _GameObjects.Length; i++)
                    {
                        int index = i;
                        GameObject _ini = _GameObjects[index];
                        Button btn = _ini.GetComponent<Button>();

                        if (btn != null)
                        {
                            btn.onClick.AddListener(() =>
                            {
                                GetIndex.Index = index;
                                method();
                            });
                        }
                    }
                }
                else
                {
                    throwError(1);
                }
            }
            else
            {
                throwError();
            }
        }

        /*
			SERVER WEBSITE
		*/
        public Arjunane GetURL(string url = null)
        {
            _field.Clear();
            _url = url;
            return this;
        }
        public Arjunane AddField(string field, string value)
        {
            _field.Add(field, value);
            return this;
        }
        public Arjunane AddFields(Dictionary<string, string> _dict)
        {
            _field = _dict;
            return this;
        }
        public IEnumerator SendForm(UnityAction<string, bool, string> callback, int timeout = 60)
        {
            // set to the server
            WWWForm form = new WWWForm();
            foreach (var item in _field)
            {
                form.AddField(item.Key, item.Value);
            }
            //Debug.Log(System.Text.Encoding.UTF8.GetString(form.data));
            UnityWebRequest proses = UnityWebRequest.Post(_url, form);
            proses.timeout = timeout;
            yield return proses.SendWebRequest();
            if (proses.isNetworkError || proses.isHttpError)
            {
                _resServer = false;
                _errorHTTP = proses.error;
                Debug.LogError(proses.error);
            }
            else
            {
                _resServer = true;
                _textHTTP = proses.downloadHandler.text;
            }
            callback(_textHTTP, _resServer, _errorHTTP);
        }
        private IEnumerator _form()
        {
            WWWForm form = new WWWForm();
            foreach (var item in _field)
            {
                form.AddField(item.Key, item.Value);
            }

            var proses = UnityWebRequest.Post(_url, form);

            yield return proses.SendWebRequest();
            isServerDone = "done";
            if (proses.isNetworkError || proses.isHttpError)
            {
                _resServer = false;
                _errorHTTP = proses.error;
                Debug.LogError(proses.error);
            }
            else
            {
                _resServer = true;
                _textHTTP = proses.downloadHandler.text;
            }
        }

        [Obsolete("Method SendWeb is deprecated, please use SendWebRequest instead.")]
        public IEnumerator SendWeb(Action<string, bool, string> callback)
        {
            WWW w3 = new WWW(_url);
            yield return w3;

            if (w3.error == null)
            {
                _resServer = true;
                _textHTTP = w3.text;
            }
            else
            {
                _resServer = false;
                _errorHTTP = w3.error;
                Debug.LogError(w3.error);
            }
            callback(_textHTTP, _resServer, _errorHTTP);
        }
        public IEnumerator SendWebRequest(Action<string, bool, string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_url))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    _resServer = false;
                    _errorHTTP = webRequest.error;
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    _resServer = true;
                    _textHTTP = webRequest.downloadHandler.text;
                }
            }
            callback(_textHTTP, _resServer, _errorHTTP);
        }

        public T[] FromJsonArray<T>(string json)
        {
            WrapperJson<T> wrapper = JsonUtility.FromJson<WrapperJson<T>>("{\"Items\":" + json + "}");
            return wrapper.Items;
        }

        public string SetTextHTTP()
        {
            return _textHTTP;
        }
        public string ShowError()
        {
            return _errorHTTP;
        }

        public bool GetResultServer()
        {
            while (isServerDone != "")
            {
                Debug.Log(isServerDone);
                return _resServer;
            }
            return true;
        }

        [Obsolete("Method LoadImage is deprecated, please use LoadImageRequest instead.")]
        public void LoadImage(GameObject raw)
        {
            server.Execute(_LoadImage(raw));
        }

        [Obsolete]
        private IEnumerator _LoadImage(GameObject raw)
        {
            RawImage ri = raw.GetComponent<RawImage>();
            WWW w3 = new WWW(_url);
            yield return w3;
            isServerDone = "done";
            if (w3.error == null)
            {
                ri.texture = w3.texture;
            }
        }
        public void LoadImageRequest(GameObject raw)
        {
            server.Execute(_LoadImageRequest(raw));
        }
        private IEnumerator _LoadImageRequest(GameObject raw)
        {
            RawImage ri = raw.GetComponent<RawImage>();
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url))
            {

                yield return webRequest.SendWebRequest();
                if (!webRequest.isNetworkError)
                {
                    ri.texture = DownloadHandlerTexture.GetContent(webRequest);
                }
            }
        }

        /*
			OTHER FUNCTION
		*/
        public string SetDate(string date, bool monthName = true, bool wib = false, string split = "-")
        {
            // split space date if time is exist
            string[] _date = date.Split(" ".ToCharArray());
            string[] _monthName = new string[12];

            string _setDate = _date[0];

            _monthName[0] = "Januari";
            _monthName[1] = "Februari";
            _monthName[2] = "Maret";
            _monthName[3] = "April";
            _monthName[4] = "Mei";
            _monthName[5] = "Juni";
            _monthName[6] = "July";
            _monthName[7] = "Agustus";
            _monthName[8] = "September";
            _monthName[9] = "Oktober";
            _monthName[10] = "November";
            _monthName[11] = "Desember";

            string[] _splitDate = _date[0].Split("-".ToCharArray());

            if (monthName == true)
            {
                _setDate = _splitDate[0] + split + _monthName[Int16.Parse(_splitDate[1])] + split + _splitDate[2];
            }

            string _wib = (wib == true) ? " WIB" : "";
            string _dateString = _setDate + " " + _date[1] + _wib;
            return _dateString;
        }
        class WrapperJson<T>
        {
            public T[] Items = null;
        }
        class GetIndex { public static int Index { get; set; } }
        class GetLength{ public static int Length { get; set; } }
        class GetElemFind
        {
            public List<GameObject> GameObjects = new List<GameObject>();
            public List<string> NotInArray = new List<string>();
        }
        class GetList
        {
            public GameObject gameObject { get; set; }
            public int sibling { get; set; }
        }
        class ServerExecute : MonoBehaviour
        {
            private class CoroutineHolder : MonoBehaviour { }

            //lazy singleton pattern. Note that I don't set it to dontdestroyonload - you usually want corotuines to stop when you load a new scene.
            private static CoroutineHolder _runner;
            private static CoroutineHolder runner
            {
                get
                {
                    if (_runner == null)
                    {
                        _runner = new GameObject("Static Corotuine Runner").AddComponent<CoroutineHolder>();
                    }
                    return _runner;
                }
            }

            public void Execute(IEnumerator corotuine)
            {
                runner.StartCoroutine(corotuine);
                DestroyImmediate(GameObject.Find("Static Corotuine Runner"));
            }
            public void Clone(GameObject child, GameObject parent)
            {
                GameObject instantiate = Instantiate(child);
                instantiate.name = instantiate.name.Replace("(Clone)", "");
                instantiate.transform.SetParent(parent.transform);
            }
        }
    }
}