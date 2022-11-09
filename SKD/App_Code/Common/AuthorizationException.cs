using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AuthorizationException
/// </summary>
public class AuthorizationException : ApplicationException
{
	public AuthorizationException()
        : base("You are not authorized to view this page")
	{

	}

}
