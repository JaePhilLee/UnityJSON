using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;          //For KeyPair Select

/*
# Update
    - 2022-01-17[jp] : 최초 제작일 (JSON 라이브러리 연동 및 indexer 구현)
    - 2022-01-17[jp] : JSON과 Dictionary 간 변환


# Reference
    - Newtonsoft.json : https://www.newtonsoft.com/json
    - Conversion between JSON Object(or Array) and Dictionary : https://stackoverflow.com/questions/14886800/convert-jobject-into-dictionarystring-object-is-it-possible


# Sample
    /////// Dictionary 기반 사용 방법 ///////
    // PxJSON json = new PxJSON( new Dictionary<string, object> {
    //     {"key1", 1},
    //     {"key2", 1.0f},
    //     {"key3", "Hello"}
    // } );
    // Debug.Log( "[" + json["type"] + "] : " + json.ToString() );

    // // 값 수정
    // if ( !json.IsArray() ) {
    //     Debug.Log( "Before key3: " + ((JObject) json["data"])["key3"] );
    //     ((JObject) json["data"])["key3"] = "hELLO";
    //     Debug.Log( "After key3: " + ((JObject) json["data"])["key3"] );
    // }
    /////////////////////////////////////////



    /////// Dictionary[] 기반 사용 방법 ///////
    // PxJSON json2 = new PxJSON( new Dictionary<string, object>[] {
    //     new Dictionary<string, object>{ {"1key1", 11}, {"1key2", 11.0f}, {"1key3", "1Hello"} },
    //     new Dictionary<string, object>{ {"2key1", 21}, {"2key2", 21.0f}, {"2key3", "2Hello"} },
    //     new Dictionary<string, object>{ {"3key1", 31}, {"3key2", 31.0f}, {"3key3", "3Hello"} }
    // });
    // Debug.Log( "[" + json2["type"] + "] : " + json2.ToString() );

    // // 값 수정
    // if ( json2.IsArray() ) {
    //     Debug.Log( "Before 2key1: " + ((JArray) json2["data"])[1]["2key1"] );
    //     ((JArray) json2["data"])[1]["2key1"] = 212121;
    //     Debug.Log( "After 2key1: " + ((JArray) json2["data"])[1]["2key1"] );
    // }
    /////////////////////////////////////////



    /////// JSON String 기반 사용 방법 ///////
    // PxJSON json3 = new PxJSON("[{'key1': 2, 'key2': 2.0, 'key3': 'World', 'key4': {'key4-1': '!!!'}, 'key5': [1, 2, 3, 4, 5]}, {'2key1': 22, '2key2': 22.0, '2key3': '2World', '2key4': {'2key4-1': '2!!!'}, '2key5': [21, 22, 23, 24, 25]}]");
    // Debug.Log( json3.ToString() );

    // // JSON 내부 접근
    // Debug.Log( "json3[1]['2key1']: " + ((JArray) json3["data"])[1]["2key1"] );
    // ((JArray) json3["data"])[1]["2key1"] = "42";
    // Debug.Log( json3.ToString() );

    // // JSON 변경
    // json3["data"] = "{'key1': 2, 'key2': 2.0, 'key3': 'World', 'key4': {'key4-1': '!!!'}, 'key5': [1, 2, 3, 4, 5]}";
    // Debug.Log( json3.ToString() );
    // json3["data"] = new JObject();
    // json3["data"] = new JArray();
    // Debug.Log( json3.ToString() );
    /////////////////////////////////////////
*/



public class PxJSON
{
    // Type에 대한 정의 ( ex: Object.GetType().FullName.Contains(TYPE_STRING) )
    public static string TYPE_STRING = "System.String";
    public static string TYPE_JSON_OBJECT = "Newtonsoft.Json.Linq.JObject";
    public static string TYPE_JSON_ARRAY = "Newtonsoft.Json.Linq.JArray";

    private bool _isArray;  // JObject or JArray
    private object _data;   // JSON 객체가 저장될 공간. ( new PxJSON()["data"] 형태로 가져올 수 있다. )

    #region 생성자
    public PxJSON() {
        _isArray = false;
        _data = null;
    }

    public PxJSON(string jsonStr) {
        _isArray = (jsonStr[0] == '[');
        _data = ToJSON( jsonStr );
    }

    public PxJSON(JObject json) {
        _isArray = false;
        _data = json;
    }

    public PxJSON(JArray json) {
        _isArray = true;
        _data = json;
    }

    public PxJSON(Dictionary<string, object> dic) {
        _isArray = false;
        _data = ToJSON( JsonConvert.SerializeObject(dic) );
    }

    public PxJSON(Dictionary<string, object>[] dicArray) {
        _isArray = true;
        _data = ToJSON( JsonConvert.SerializeObject(dicArray) );
    }
    #endregion


    #region Public Methods
    public object this[string key]
    {
        get
        {
            if ( key == "data" ) {
                return _data;
            } else if ( key == "type" ) {
                return _data.GetType().FullName;
            }

            return null;
        }
        set
        {
            string type = value.GetType().FullName;

            if ( type.Contains(TYPE_STRING) ) {
                // Debug.Log("Set TYPE_STRING");
                _isArray = false;
                _data = ToJSON( (string) value );
            } else if ( type.Contains(TYPE_JSON_OBJECT) ) {
                // Debug.Log("Set TYPE_JSON_OBJECT");
                _isArray = false;
                _data = value;
            } else if ( type.Contains(TYPE_JSON_ARRAY) ) {
                // Debug.Log("Set TYPE_JSON_ARRAY");
                _isArray = true;
                _data = value;
            } else {
                Debug.Log("JSON Set Failed. [PxJSON.cs] \n\t - Unknown Type [" + type + "]");
            }
        }
    }

    // JSON String => JSON Object(or Array)
    public object ToJSON(string jsonStr)
    {
        if ( jsonStr[0] == '{' ) {
            return JsonConvert.DeserializeObject<JObject>(jsonStr);
        } else if ( jsonStr[0] == '[' ){
            return JsonConvert.DeserializeObject<JArray>(jsonStr);
        } else {
            return null;
        }
    }

    // JSON Object(or Array) => String
    public string ToString() {
        if ( _data == null ) {
            return "";
        }

        return JsonConvert.SerializeObject(_data);
    }

    // JSON Object인지 JSON Array인지 판별
    public bool IsArray() {
        return _isArray;
    }

    // JSON Object를 Dictionary 형태로 변경
    public Dictionary<string, object> ToDictionary(JObject json = null)
    {
        if ( json == null ) json = (JObject) _data;

        var propertyValuePairs = json.ToObject<Dictionary<string, object>>();
        ProcessJObjectProperties(propertyValuePairs);
        ProcessJArrayProperties(propertyValuePairs);
        return propertyValuePairs;
    }

    // JSON Array를 Dictionary Array 형태로 변경
    public object[] ToDictionaryArray(JArray array = null)
    {
        if ( array == null ) array = (JArray) _data;

        return array.ToObject<object[]>().Select(ProcessArrayEntry).ToArray();
    }

    #endregion

    #region Private Methods
    private void ProcessJObjectProperties(Dictionary<string, object> propertyValuePairs)
    {
        var objectPropertyNames = (from property in propertyValuePairs
            let propertyName = property.Key
            let value = property.Value
            where value is JObject
            select propertyName).ToList();

        objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject) propertyValuePairs[propertyName]));
    }

    private void ProcessJArrayProperties(Dictionary<string, object> propertyValuePairs)
    {
        var arrayPropertyNames = (from property in propertyValuePairs
            let propertyName = property.Key
            let value = property.Value
            where value is JArray
            select propertyName).ToList();

        arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionaryArray((JArray) propertyValuePairs[propertyName]));
    }

    private object ProcessArrayEntry(object value)
    {
        if (value is JObject)
        {
            return ToDictionary((JObject) value);
        }
        if (value is JArray)
        {
            return ToDictionaryArray((JArray) value);
        }
        return value;
    }
    #endregion
}