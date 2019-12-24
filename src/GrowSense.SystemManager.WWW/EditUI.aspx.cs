using GrowSense.SystemManager.Web;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class EditUI : BaseEditDevicePage
  {
    public override void Construct ()
    {
      EnableReadingInterval = false;
      
      base.Construct ();
    }

    public override void InitializeForm ()
    {
      base.InitializeForm ();
    }

    public override void PopulateForm ()
    {
      base.PopulateForm ();
    }

    public override void HandleSubmission ()
    {     
      base.HandleSubmission ();
    }
  }
}

