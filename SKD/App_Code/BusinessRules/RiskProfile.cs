using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Transactions;

/// <summary>
/// Summary description for RiskProfile
/// </summary>
public class RiskProfile
{
	
    #region "-- RiskProfile --"
    public static RiskProfileData.RiskProfileRow GetRiskProfileByRiskProfileID(decimal riskProfileID)
    {
        RiskProfileData.RiskProfileRow dr = null;
        RiskProfileData.RiskProfileDataTable dt = new RiskProfileData.RiskProfileDataTable();
        RiskProfileDataTableAdapters.RiskProfileTableAdapter ta = new RiskProfileDataTableAdapters.RiskProfileTableAdapter();

        try
        {
            ta.FillByRiskProfileID(dt, riskProfileID);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static RiskProfileData.RiskProfileDataTable 
        GetRiskProfileByClearingMemberID(Nullable<decimal> clearingMemberID)
    {
        RiskProfileData.RiskProfileDataTable dt = new RiskProfileData.RiskProfileDataTable();
        RiskProfileDataTableAdapters.RiskProfileTableAdapter ta = new RiskProfileDataTableAdapters.RiskProfileTableAdapter();

        try
        {
            ta.FillByCMID(dt, clearingMemberID);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static RiskProfileData.RiskProfileDataTable ValidateUpdateRiskProfile(decimal clearingMemberID, 
        DateTime startDate, DateTime endDate)
    {
        RiskProfileData.RiskProfileDataTable dt = new RiskProfileData.RiskProfileDataTable();
        RiskProfileDataTableAdapters.RiskProfileTableAdapter ta = new RiskProfileDataTableAdapters.RiskProfileTableAdapter();

        try
        {
            ta.FillValidateByStartAndEndDate(dt, clearingMemberID, startDate, endDate);
           
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void UpdateRiskProfile(decimal clearingMemberID, DateTime startDate, DateTime endDate, decimal riskProfileID, 
        string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate, 
        RiskProfileData.RiskProfileDetailDataTable dtNewRiskProfileDetail)
    {
        //Risk Profile
        RiskProfileDataTableAdapters.RiskProfileTableAdapter taRiskProfile = new RiskProfileDataTableAdapters.RiskProfileTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {                
                //Check Risk Profile : If nothing then insert else update
                if (riskProfileID == 0)
                {
                    riskProfileID = (decimal)taRiskProfile.InsertRiskProfile(clearingMemberID, startDate, endDate,
                        createdBy, createdDate, lastUpdatedBy, lastUpdatedDate);
                }
                else
                {
                    taRiskProfile.UpdateRiskProfile(startDate, endDate, lastUpdatedBy, lastUpdatedDate, 
                        riskProfileID);
                }

                //Risk Profile Detail
                RiskProfileData.RiskProfileDetailRow drNewRiskProfileDetail = null;
                RiskProfileDataTableAdapters.RiskProfileDetailTableAdapter taRiskDetail = 
                    new RiskProfileDataTableAdapters.RiskProfileDetailTableAdapter();
                RiskProfileData.RiskProfileDetailDataTable dtRiskDetail = 
                    new RiskProfileData.RiskProfileDetailDataTable();

                taRiskDetail.FillByRiskProfileID(dtRiskDetail, riskProfileID);
                
                //Process Risk Profile Detail : add/update/delete data
                foreach (RiskProfileData.RiskProfileDetailRow drRiskDetail in dtNewRiskProfileDetail)
                {
                    drNewRiskProfileDetail = dtRiskDetail.FindByRiskProfileIDRiskTypeID(riskProfileID, drRiskDetail.RiskTypeID);

                    if (drNewRiskProfileDetail == null)
                    {
                        drNewRiskProfileDetail = dtRiskDetail.NewRiskProfileDetailRow();
                    }

                    drNewRiskProfileDetail.RiskProfileID = riskProfileID;
                    drNewRiskProfileDetail.RiskTypeID = drRiskDetail.RiskTypeID;
                    drNewRiskProfileDetail.Impact = drRiskDetail.Impact;
                    drNewRiskProfileDetail.Likelihood = drRiskDetail.Likelihood;

                    if (drNewRiskProfileDetail.RowState == DataRowState.Detached)
                    {
                        dtRiskDetail.AddRiskProfileDetailRow(drNewRiskProfileDetail);
                    }
                }

                foreach (RiskProfileData.RiskProfileDetailRow drRiskDetail in dtRiskDetail)
                {
                    if (drRiskDetail.RowState == DataRowState.Unchanged)
                    {
                        drRiskDetail.Delete();
                    }
                }

                taRiskDetail.Update(dtRiskDetail);


                string log = string.Format("CMID:{0}|StartDate:{1}|EndDate:{2}|RiskProfileID:{3}", clearingMemberID, startDate, endDate,riskProfileID);
                AuditTrail.AddAuditTrail("RiskProfile", "Insert", log, createdBy,"Insert");

                scope.Complete();               
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                throw new ApplicationException("Clearing member with this start date already exist.");
            }
            else
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
        
    }

    public static void DeleteRiskProfileByRiskProfileID(decimal RiskProfileID)
    {
        RiskProfileDataTableAdapters.RiskProfileTableAdapter ta = new RiskProfileDataTableAdapters.RiskProfileTableAdapter();

        using (TransactionScope scope = new TransactionScope())
        {
            DeleteRiskProfileDetailByRiskProfileID(RiskProfileID);
            ta.DeleteByRiskProfileID(RiskProfileID);

            scope.Complete();
        }
    }
    #endregion

    #region "-- RiskProfileDetail --"
    public static RiskProfileData.RiskProfileDetailDataTable 
        GetRiskProfileDetailByRiskProfileID(decimal riskProfileID)
    {
        RiskProfileData.RiskProfileDetailDataTable dt = new RiskProfileData.RiskProfileDetailDataTable();
        RiskProfileDataTableAdapters.RiskProfileDetailTableAdapter ta = new RiskProfileDataTableAdapters.RiskProfileDetailTableAdapter();

        try
        {
            ta.FillByRiskProfileID(dt, riskProfileID);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void DeleteRiskProfileDetailByRiskProfileID(decimal RiskProfileID)
    {
        RiskProfileDataTableAdapters.RiskProfileDetailTableAdapter ta = new RiskProfileDataTableAdapters.RiskProfileDetailTableAdapter();

        ta.DeleteByRiskProfileID(RiskProfileID);
    }

    
    #endregion

    #region "-- RiskProfileType --"
    public static RiskProfileData.RiskTypeDataTable GetRiskType()
    {
        RiskProfileData.RiskTypeDataTable dt = new RiskProfileData.RiskTypeDataTable();
        RiskProfileDataTableAdapters.RiskTypeTableAdapter ta = new RiskProfileDataTableAdapters.RiskTypeTableAdapter();

        try
        {
            ta.Fill(dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetRiskTypeByRiskTypeID(decimal riskTypeID)
    {
        RiskProfileData.RiskTypeDataTable dt = new RiskProfileData.RiskTypeDataTable();
        RiskProfileData.RiskTypeRow dr = null;
        RiskProfileDataTableAdapters.RiskTypeTableAdapter ta = new RiskProfileDataTableAdapters.RiskTypeTableAdapter();
        string riskType = "";
        try
        {
            ta.FillByRiskTypeID(dt, riskTypeID);
            if (dt.Count > 0)
            {
                dr = dt[0];
                riskType = dr.RiskType;                
            }

            return riskType;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion

    #region "-- RiskColor --"
    public static RiskProfileData.RiskColorDataTable GetRiskColorByImpactAndLikelihood(int impact, int likelihood)
    {
        RiskProfileData.RiskColorDataTable dt = new RiskProfileData.RiskColorDataTable();
        RiskProfileDataTableAdapters.RiskColorTableAdapter ta = new RiskProfileDataTableAdapters.RiskColorTableAdapter();

        try
        {
            ta.FillByImpactAndLikelihood(dt, impact, likelihood);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static RiskProfileData.RiskColorDataTable GetRiskColorByApprovalStatus(string approvalStatus)
    {
        RiskProfileData.RiskColorDataTable dt = new RiskProfileData.RiskColorDataTable();
        RiskProfileDataTableAdapters.RiskColorTableAdapter ta = new RiskProfileDataTableAdapters.RiskColorTableAdapter();

        try
        {
            ta.FillByApprovalStatus(dt, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void InsertRiskColor(int impact, int likelihood, string approvalStatus, 
        string riskColor, string createdBy, DateTime createdDate, string lastUpdatedBy, 
        DateTime lastUpdatedDate, string approvalDesc)
    {
        RiskProfileDataTableAdapters.RiskColorTableAdapter ta = new RiskProfileDataTableAdapters.RiskColorTableAdapter();
        
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(impact, likelihood, approvalStatus, riskColor, createdBy, createdDate,
                    lastUpdatedBy, lastUpdatedDate, approvalDesc);

                string log = string.Format("Impact:{0}|Likelihood:{1}|RiskColor:{2}", impact, likelihood, riskColor);
                AuditTrail.AddAuditTrail("RiskColor", AuditTrail.PROPOSE, log, createdBy,"Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                throw new ApplicationException("This record is already exist.", ex);
            }
            else
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }

    public static void UpdateRiskColor(string riskColor, string lastUpdatedBy, 
        DateTime lastUpdatedDate, string approvalDesc, string ApprovalStatus, int impact, int likelihood)
    {
        RiskProfileDataTableAdapters.RiskColorTableAdapter ta = new RiskProfileDataTableAdapters.RiskColorTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.UpdateRiskColor(riskColor, lastUpdatedBy, lastUpdatedDate,
                    approvalDesc, ApprovalStatus, impact, likelihood);

                string log = log = string.Format("Impact:{0}|Likelihood:{1}|RiskColor:{2}", impact, likelihood, riskColor);
                if (ApprovalStatus == "A")
                {
                    AuditTrail.AddAuditTrail("RiskColor", AuditTrail.APPROVE, log, lastUpdatedBy,"Approve Update");
                }
                else if (ApprovalStatus == "R")
                {
                    AuditTrail.AddAuditTrail("RiskColor", AuditTrail.REJECT, log, lastUpdatedBy,"Reject Update");
                }
                else if (ApprovalStatus == "P")
                {
                    AuditTrail.AddAuditTrail("RiskColor", AuditTrail.PROPOSE, log, lastUpdatedBy, "Propose Update");
                }

                scope.Complete();
            }
            
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void DeleteRiskColor(int Impact, int Likelihood, string ApprovalStatus)
    {
        RiskProfileDataTableAdapters.RiskColorTableAdapter ta = new RiskProfileDataTableAdapters.RiskColorTableAdapter();

        ta.Delete(Impact, Likelihood, ApprovalStatus);

    }
    #endregion
}
