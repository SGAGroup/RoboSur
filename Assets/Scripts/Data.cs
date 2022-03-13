using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Com.sgagdr.BlackSky
{
    public class Data : MonoBehaviour
    {
        public static void SaveProfile(ProfileData p_profile)
        {
            string path = Application.persistentDataPath + "/profile.data";
            try
            {

                if (File.Exists(path)) File.Delete(path);

                FileStream file = File.Create(path);
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(file, p_profile);
                file.Close();
            }
            catch
            {
                Debug.Log("Something went wrong with SaveProfile(), Data.cs");
            }
        }

        public static ProfileData LoadProfile()
        {
            ProfileData result = new ProfileData();

            try
            {

                string path = Application.persistentDataPath + "/profile.data";
                if (File.Exists(path))
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    FileStream file = File.Open(path, FileMode.Open);
                    result = (ProfileData)bf.Deserialize(file);

                }
            }
            catch
            {
                Debug.Log("Catch error while opening ProfileData. Going to return empty profile");
            }

            return result;
        }

    }
}
