using BestNox.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestNox.Controllers
{
    public class ControllerHelper
    {
        #region GetSubmitLocked:投稿ロックモードかどうかを取得する
        /// <summary>
        /// 投稿ロックモードかどうかを取得する
        /// </summary>
        /// <returns>"1"ならば投稿禁止</returns>
        public static string GetSubmitLocked(ApplicationDbContext _context)
        {
            var list = _context.SystemParameters.Where(d => !d.IsDeleted && d.CategoryId == SystemConstants.SystemPatameterMode && d.OrderNo == SystemConstants.SystemPatameterModeDemo).ToList();
            if (list.Count == 0
                || list.Count > 0 && list[0].CurrentValue == "1")
            {
                // 投稿のロック
                return "1";
            }
            return "0";
        }
        #endregion

    }
}
