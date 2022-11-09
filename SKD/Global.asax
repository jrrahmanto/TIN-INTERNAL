<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Server.Transfer("~/ErrorPage.aspx");

        Server.ClearError();
        //Session["LastError"] = Server.GetLastError();
        //Session["LastErrorUrl"] = Request.Url.ToString();

        Server.Transfer("~/ErrorPage.aspx");
        Server.ClearError();
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
       
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        //Session.Clear();
        //FormsAuthentication.SignOut();
        //Response.Redirect("~/Login.aspx?reason=1");
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
    }

    protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated && Context.Session != null)
        {
            if (Session[User.Identity.Name] == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx?reason=1&ReturnUrl=" + Request.RawUrl);
            }
            
            // Validation for same user is logged in from different client
            if (Session[User.Identity.Name] != Application[User.Identity.Name])
            {
                Session[User.Identity.Name] = "";
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx?reason=2&ReturnUrl=" + Request.RawUrl);
            }

            // Validation for user try to use application in unauthorized time permitted
            if (Session["StartTime"] != null)
            {
                DateTime userLoginStartTime = DateTime.Parse(Session["StartTime"].ToString());
                DateTime userLoginEndTime = DateTime.Parse(Session["EndTime"].ToString());

                if (DateTime.Now >= userLoginStartTime && DateTime.Now <= userLoginEndTime)
                {
                    //skip
                }
                else
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Login.aspx?reason=3&ReturnUrl=" + Request.RawUrl);
                }
            }
            
        }
    }
    
    

</script>
