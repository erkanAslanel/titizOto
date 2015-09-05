using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HelperSite.DbController;
using ImageResizer;


namespace titizOto.Controllers
{
    public class ImageShowController : Controller
    {

        #region WaterMark


        public ActionResult ShowGuidWithWatermark(string guid, string path, string width, string height)
        {
            int intWidth = 0;
            int intHeight = 0;

            if (!int.TryParse(width, out intWidth))
            {
                return null;
            }

            if (!int.TryParse(height, out intHeight))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(path))
            {
                return null;
            }


            string photoName = "";
            string coordinate = "";

            var dc = new DbWithBasicFunction();
            var db = dc.db;

            if (path.ToLower() == "gallery")
            {
                var galleryItem = db.tbl_gallery.Where(a => a.guid == guid).FirstOrDefault();

                if (galleryItem == null)
                {
                    return null;
                }

                photoName = galleryItem.photo;
                coordinate = galleryItem.photoCoordinate;

            }


            var item = getFileCoordinated(photoName, coordinate, path);


            if (item != null)
            {
                var settings = new ResizeSettings();

                int widthInt = int.Parse(width);
                int heightInt = int.Parse(height);

                settings.Width = widthInt;
                settings.Height = heightInt;
                settings.Mode = FitMode.Stretch;

                WebImage waterMarkImage;

                using (MemoryStream st = new MemoryStream())
                {

                    ImageBuilder.Current.Build(item.First().Key, st, settings);
                    st.Position = 0;

                    waterMarkImage = new WebImage(st);

                }

                waterMarkImage.AddTextWatermark("www.titizkromaksesuar.com", fontColor: "#ffe27b", fontSize: 10, verticalAlign: "Middle", opacity: 60);

                return File(waterMarkImage.GetBytes(), item.First().Value);

            }
            else
            {
                return null;
            }

        }

        public ActionResult ShowGuidWithoutWatermark(string guid, string path, string width, string height)
        {
            int intWidth = 0;
            int intHeight = 0;

            if (!int.TryParse(width, out intWidth))
            {
                return null;
            }

            if (!int.TryParse(height, out intHeight))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(path))
            {
                return null;
            }


            string photoName = "";
            string coordinate = "";

            var dc = new DbWithBasicFunction();
            var db = dc.db;

            if (path.ToLower() == "gallery")
            {
                var galleryItem = db.tbl_gallery.Where(a => a.guid == guid).FirstOrDefault();

                if (galleryItem == null)
                {
                    return null;
                }

                photoName = galleryItem.photo;
                coordinate = galleryItem.photoCoordinate;

            }


            var item = getFileCoordinated(photoName, coordinate, path);


            if (item != null)
            {
                var settings = new ResizeSettings();

                int widthInt = int.Parse(width);
                int heightInt = int.Parse(height);

                settings.Width = widthInt;
                settings.Height = heightInt;
                settings.Mode = FitMode.Stretch;

                byte[] fileByte;
                using (MemoryStream st = new MemoryStream())
                {
                    ImageBuilder.Current.Build(item.First().Key, st, settings);
                    st.Position = 0;

                    fileByte = st.ToArray();
                }

                return File(fileByte, item.First().Value);
            }
            else
            {
                return null;
            }

        }

        #endregion 

        public ActionResult Resize(string photoName, string path, string width, string height)
        {
            int intWidth = 0;
            int intHeight = 0;

            if (!int.TryParse(width, out intWidth))
            {
                return null;
            }

            if (!int.TryParse(height, out intHeight))
            {
                return null;
            }


            var item = getFileResized(photoName, path, intWidth, intHeight, FitMode.Crop);

            return File(item.First().Key, item.First().Value);

        }

        public ActionResult Crop(string photoName, string coordinate, string path)
        {
            var item = getFileCoordinated(photoName, coordinate, path);

            if (item != null)
            {
                return base.File(item.First().Key, item.First().Value);
            }
            else
            {
                return null;
            }

        }

        public ActionResult CropExact(string photoName, string coordinate, string path, string width, string height)
        {
            var item = getFileCoordinated(photoName, coordinate, path);
            return getResizeFile(item, width, height, FitMode.Crop);

        }

        public ActionResult CropExactExactImage(string photoName, string coordinate, string path, string width, string height)
        {
            var item = getFileCoordinated(photoName, coordinate, path);
            return getResizeFile(item, width, height, FitMode.Stretch);

        }


        public Dictionary<string, string> getFileCoordinated(string photoName, string coordinate, string path)
        {
            Dictionary<string, string> returnItem = new Dictionary<string, string>();


            if (string.IsNullOrWhiteSpace(photoName) || string.IsNullOrWhiteSpace(coordinate) || string.IsNullOrWhiteSpace(path))
            {
                return null;
            }


            List<int> coordinateIntList = getCordinateToIntList(coordinate);

            if (coordinateIntList == null || coordinateIntList.Count != 6)
            {
                return null;
            }

            var fileProperty = getExtenstionWithFileExistControl(path, photoName);


            // if File is Valid
            if (fileProperty.Item1)
            {
                string resultExtention = fileProperty.Item2;
                string filePath = fileProperty.Item3;
                string withoutCommaCoordinate = coordinate.Replace(",", string.Empty);
                string filePathWithCoordiante = Server.MapPath("~/" + "Download/item/" + path + "/" + withoutCommaCoordinate + "_" + photoName);

                if (System.IO.File.Exists(filePathWithCoordiante))
                {
                    returnItem.Add(filePathWithCoordiante, resultExtention);
                    return returnItem;
                }
                else
                {

                    var settings = new ResizeSettings("crop=(" + coordinateIntList[0].ToString() + "," + coordinateIntList[1].ToString() + "," + coordinateIntList[2].ToString() + "," + coordinateIntList[3].ToString() + ")");

                    settings.Quality = 100;
                    string savedPath = Server.MapPath("~/" + "Download/item/" + path + "/" + withoutCommaCoordinate + "_" + photoName);

                    ImageBuilder.Current.Build(filePath, savedPath,
                       settings);

                    returnItem.Add(savedPath, resultExtention);
                    return returnItem;

                }


            }

            return null;

        }

        public Dictionary<string, string> getFileResized(string photoName, string path, int width, int height, FitMode mode)
        {
            Dictionary<string, string> returnItem = new Dictionary<string, string>();


            if (string.IsNullOrWhiteSpace(photoName) || string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var fileProperty = getExtenstionWithFileExistControl(path, photoName);

            if (fileProperty.Item1)
            {

                string resultExtention = fileProperty.Item2;
                string filePath = fileProperty.Item3;
                string withoutCommaCoordinate = width.ToString() + height.ToString();
                string filePathWithCoordiante = Server.MapPath("~/" + "Download/item/" + path + "/" + withoutCommaCoordinate + mode.ToString() + "_" + photoName);


                if (System.IO.File.Exists(filePathWithCoordiante))
                {
                    returnItem.Add(filePathWithCoordiante, resultExtention);
                    return returnItem;
                }
                else
                {

                    var settings = new ResizeSettings();

                    settings.Width = width;
                    settings.Height = height;
                    settings.Mode = mode;
                    settings.Quality = 100;

                    ImageBuilder.Current.Build(filePath, filePathWithCoordiante,
                       settings);

                    returnItem.Add(filePathWithCoordiante, resultExtention);
                    return returnItem;

                }
            }

            return null;
        }

        public FileResult getResizeFile(Dictionary<string, string> item, string width, string height, FitMode mode)
        {

            if (item != null)
            {
                var settings = new ResizeSettings();

                int intWidth = 0;
                int intHeight = 0;

                if (!int.TryParse(width, out intWidth))
                {
                    return null;
                }

                if (!int.TryParse(height, out intHeight))
                {
                    return null;
                }

                settings.Width = intWidth;
                settings.Height = intHeight;
                settings.Quality = 100;
                settings.Mode = mode;


                byte[] fileByte;
                using (MemoryStream st = new MemoryStream())
                {
                    ImageBuilder.Current.Build(item.First().Key, st, settings);
                    st.Position = 0;

                    fileByte = st.ToArray();
                }

                return File(fileByte, item.First().Value);
            }
            else
            {
                return null;
            }
        }

        public List<int> getCordinateToIntList(string cordinateString)
        {
            string[] coordinateArray = cordinateString.Split(',');


            if (coordinateArray.Length != 6)
            {
                return null;
            }

            List<int> coordinateIntList = new List<int>();

            foreach (var item in coordinateArray)
            {
                decimal addValue;
                if (decimal.TryParse(item, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out addValue))
                {
                    if (addValue < 0)
                    {
                        coordinateIntList.Add(0);
                    }
                    else
                    {
                        coordinateIntList.Add(Convert.ToInt32(addValue));
                    }

                }
                else
                {
                    return null;
                }
            }

            return coordinateIntList;

        }

        /// <summary>
        /// item1 => imageIsValid
        /// item2 => extention
        /// item3 => filePath
        /// </summary>  
        public Tuple<bool, string, string> getExtenstionWithFileExistControl(string path, string photoName)
        {
            string resultExtention = null;
            bool isImage = false;

            string filePath = Server.MapPath("~/" + "Download/item/" + path + "/" + photoName);

            if (System.IO.File.Exists(filePath))
            {
                string extention = System.IO.Path.GetExtension(filePath).ToLower();
                isImage = true;

                switch (extention)
                {
                    case ".jpg":
                        resultExtention = "image/jpg";
                        break;
                    case ".png":
                        resultExtention = "image/png";
                        break;
                    case ".jpeg":
                        resultExtention = "image/jpeg";
                        break;
                    default:

                        isImage = false;
                        break;
                }
            }


            return new Tuple<bool, string, string>(isImage, resultExtention, filePath);

        }

      

    }
}
