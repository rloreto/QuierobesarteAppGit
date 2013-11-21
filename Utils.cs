using Newtonsoft.Json;
using QuierobesarteApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace QuierobesarteApp
{
    public class Utils
    {
        public static async void SerializeAppData(AppModel appModel)
        {
            
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFile = await localFolder.CreateFileAsync("app.json", CreationCollisionOption.ReplaceExisting);
            // Open the file...      
            using (IRandomAccessStream textStream = await textFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                // write the JSON string!
                using (DataWriter textWriter = new DataWriter(textStream))
                {
                    textWriter.WriteString(JsonConvert.SerializeObject(appModel));
                    await textWriter.StoreAsync();
                }
            }
        }

        public static async Task<AppModel> DeserializeAppData()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            AppModel appModel = null;
            try
            {
                // Getting JSON from file if it exists, or file not found exception if it does not  
                StorageFile textFile = await localFolder.GetFileAsync("app.json");
                using (IRandomAccessStream textStream = await textFile.OpenReadAsync())
                {
                    // Read text stream     
                    using (DataReader textReader = new DataReader(textStream))
                    {
                        //get size                       
                        uint textLength = (uint)textStream.Size;
                        await textReader.LoadAsync(textLength);
                        // read it                    
                        string jsonContents = textReader.ReadString(textLength);
                        // deserialize back to our product!  
                        appModel = JsonConvert.DeserializeObject<AppModel>(jsonContents);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return appModel;
        }
    }
}
