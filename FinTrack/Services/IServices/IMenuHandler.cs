using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTrack.Helper;

namespace FinTrack.Services.IServices
{
    public interface IMenuHandler
    {
        public void HandleMenuFlyoutItemClicked(object sender, MenuFlyoutItemClickedEventArgs e);
    }
}
