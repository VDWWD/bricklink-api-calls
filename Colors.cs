using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public static class Colors
    {
        /// <summary>
        /// Get the color data of a specific color.
        /// </summary>
        /// <param name="colorID">The BrickLink color ID.</param>
        /// <returns>VdwwdBrickLink.Classes.Color</returns>
        public static async Task<Classes.Color> GetColor(int colorID)
        {
            //check the color range
            if (colorID < Variables.ItemColorDefault || colorID > Variables.ItemColorMaxValue)
            {
                return null;
            }

            //get the color
            var colors = await GetColors(colorID);

            if (colors == null || !colors.Any())
            {
                return null;
            }

            //return the color
            return colors[0];
        }


        /// <summary>
        /// Get all the color data.
        /// </summary>
        /// <returns>List<VdwwdBrickLink.Classes.Color></returns>
        public static async Task<List<Classes.Color>> GetColors()
        {
            //return the colors
            return await GetColors(0);
        }


        /// <summary>
        /// Get the color data or a specific color.
        /// </summary>
        /// <param name="colorID">The BrickLink color ID, use 0 for all colors.</param>
        /// <returns>List<VdwwdBrickLink.Classes.Color></returns>
        private static async Task<List<Classes.Color>> GetColors(int colorID)
        {
            string url = $"/colors";

            if (colorID > 0)
            {
                url += "/" + colorID;
            }

            //call the api
            var result = await Api.callApi(url);

            //deserialize the response
            var data = JsonConvert.DeserializeObject<Classes.ColorRoot>(result);

            //if the result is not null and contains an item then return the colors
            if (data != null && data.data != null && data.data.Any())
            {
                return data.data;
            }
            else
            {
                return null;
            }
        }
    }
}
