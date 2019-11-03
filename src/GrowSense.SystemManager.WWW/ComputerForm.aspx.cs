using GrowSense.SystemManager.Computers;
using System.IO;
using System.Configuration;

namespace GrowSense.SystemManager.WWW
{
  using System;
  using System.Web;
  using System.Web.UI;

  public partial class ComputerForm : System.Web.UI.Page
  {
    public ComputerManager Manager;
    public ComputerInfo Computer;
    public string Action;
    public string ErrorMessage = String.Empty;

    public void Page_Load (object sender, EventArgs e)
    {
      var indexDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["IndexDirectory"]);
    
      var computersDirectory = Path.GetFullPath (ConfigurationSettings.AppSettings ["ComputersDirectory"]);
    
      Manager = new ComputerManager (indexDirectory, computersDirectory);
     
      var computerName = Request.QueryString ["ComputerName"];
  
      if (!String.IsNullOrEmpty (computerName)) {
        Computer = Manager.GetComputerInfo (computerName);
        Action = "Edit";
      } else
        Action = "Add";
    
      if (!IsPostBack) {
        PopulateForm ();
      } else {
        HandleSubmission ();
      }
    }

    public void PopulateForm ()
    {
      if (!String.IsNullOrEmpty (Request.QueryString ["ComputerName"]))
        Computer = Manager.GetComputerInfo (Request.QueryString ["ComputerName"]);
    
      ComputerName.Text = Computer.Name;
      ComputerPath.Text = Computer.Host;
      Username.Text = Computer.Username;
    }

    public void HandleSubmission ()
    {
      Computer.Name = ComputerName.Text;
      Computer.Host = ComputerPath.Text;
      Computer.Username = Username.Text;
      if (!String.IsNullOrEmpty (Password.Text))
        Computer.Password = Password.Text;
      Computer.Port = Convert.ToInt32 (Port.Text);
      
      ComputerActionResult result;
      if (Action == "Add") {
        result = Manager.AddComputer (Computer);
      } else {
        result = Manager.UpdateComputer (Request.QueryString ["ComputerName"], Computer);
      }
      
      if (result.IsSuccess) {
        var messageAction = "";
        if (Action == "Add")
          messageAction = "added";
        else
          messageAction = "updated";
          
        var resultMessage = ("Computer with name '" + Computer.Name + "' and host path '" + Computer.Host + "' " + messageAction + " successfully!").Replace (" ", "+");
        Response.Redirect ("Computers.aspx?Result=" + resultMessage);
      } else {
        switch (result.Error) {
        case ComputerActionError.ConnectionFailed:
          ErrorMessage = "Connection to the computer failed. Check the details and try again.";
          break;
        case ComputerActionError.PingFailed:
          ErrorMessage = "Failed to ping the computer. Check the host path.";
          break;
        case ComputerActionError.NameInUse:
          ErrorMessage = "The name of the computer is already in use. Try a different name.";
          break;
        case ComputerActionError.HostAlreadyExists:
          ErrorMessage = "A computer with the same host path has already been added.";
          break;
        case ComputerActionError.PermissionDenied:
          ErrorMessage = "Permission denied. Check the username and password.";
          break;
        }
      }
    }
  }
}

