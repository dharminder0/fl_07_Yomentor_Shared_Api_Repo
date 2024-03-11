namespace Core.Common.Extensions {
    public static class JsonSerializeDeserialize {


        public static string SerializeLeadObjectWithoutCVandNull(this object objectDetailData) {
            if (objectDetailData == null) {
                return null;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            try {

                if (objectDetailData.GetType().Name.Contains("Dictionary")) {
                    Dictionary<string, object> exObj;
                    exObj = ((Dictionary<string, object>)(objectDetailData)).DeepCopySerialization();

                    foreach (var exObjProp in exObj) {
                        if (exObjProp.Value == null) {
                            exObj.Remove(exObjProp.Key);
                        }
                    }

                    exObj.AddORUpdateDictionaryObjectkey("CVFileData", null);
                    return JsonConvert.SerializeObject(exObj, settings);
                }
                else if (objectDetailData.GetType().Name.Contains("JObject")) {

                    var jobject = ((JObject)(objectDetailData)).DeepCopySerialization();
                    SetNullToProperty(jobject, "CVFileData");
                    RemoveNullPropertiesFromJObject(jobject);
                    return JsonConvert.SerializeObject(jobject, settings);
                }


            } catch {
            }


            return JsonConvert.SerializeObject(objectDetailData, settings);

        }


        public static void RemoveNullPropertiesFromJObject(JObject jObject) {
            var propertiesToRemove = jObject.Properties().Where(p => p.Value.Type == JTokenType.Null).ToList();

            foreach (var prop in propertiesToRemove) {
                prop.Remove();
            }
        }
        public static void SetNullToProperty(JObject jObject, string propertyName) {
            // Set the property to null, or add it if it doesn't exist
            jObject[propertyName] = null;
        }

        public static string SerializeObjectWithoutNull(this object objectDetails) {
            if (objectDetails == null) {
                return null;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            try {

                if (objectDetails.GetType().Name.Contains("Dictionary")) {
                    Dictionary<string, object> exObj;
                    exObj = (Dictionary<string, object>)(objectDetails);

                    foreach (var exObjProp in exObj) {
                        if (exObjProp.Value == null) {
                            exObj.Remove(exObjProp.Key);
                        }
                    }
                    return JsonConvert.SerializeObject(exObj, settings);
                }
                else if (objectDetails.GetType().Name.Contains("JObject")) {

                    var jobject = ((JObject)(objectDetails));
                    RemoveNullPropertiesFromJObject(jobject);
                    return JsonConvert.SerializeObject(jobject, settings);
                }

            } catch {
            }


            return JsonConvert.SerializeObject(objectDetails, settings);

        }
        public static string SerializeObjectWithNull(this object objectDetails) {
            if (objectDetails == null) {
                return null;
            }

            return JsonConvert.SerializeObject(objectDetails);
        }

        public static T DeserializeObjectWithoutNull<T>(this object objectDetails) {
            if (objectDetails == null) {
                return (T)objectDetails;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<T>(objectDetails.ToString(), settings);
        }

        public static T DeserializeObjectWithNull<T>(this object objectDetails) {
            return JsonConvert.DeserializeObject<T>(objectDetails.ToString());
        }



        //      public static object SerializeDeserializeWithoutNull(this object objectDetails) {
        //    if (objectDetails == null) {
        //	    return null;
        //    }
        //          try {
        //		JsonSerializerSettings settings = new JsonSerializerSettings {
        //			NullValueHandling = NullValueHandling.Ignore
        //		};

        //		object result = objectDetails;
        //		if (objectDetails is Dictionary<string, object> exObj) {
        //			// If the object is a dictionary, remove entries with null values
        //			var filteredExObj = new Dictionary<string, object>(exObj);
        //			foreach (var exObjProp in exObj) {
        //				if (exObjProp.Value == null) {
        //					filteredExObj.Remove(exObjProp.Key);
        //				}
        //			}
        //			result = filteredExObj;
        //		}

        //		string jsonString = JsonConvert.SerializeObject(result, settings);
        //		var response = JsonConvert.DeserializeObject<object>(jsonString);
        //		return response;
        //	}catch(Exception ex) {
        //              return objectDetails;
        //          }


        //}


    }
}
