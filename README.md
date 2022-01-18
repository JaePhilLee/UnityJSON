# UnityJSON


# Reference
 - Newtonsoft.json : https://www.newtonsoft.com/json
 - Conversion between JSON Object(or Array) and Dictionary : https://stackoverflow.com/questions/14886800/convert-jobject-into-dictionarystring-object-is-it-possible
 
 
# Example I (Dictionary based)
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
    
    
 # Example II (Dictionary[] based)
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
 
 
 # Example III (JSON String based)
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
