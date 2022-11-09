using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Bank
/// </summary>
public class WithDrawal
{

    # region "-- WithDrawal --"
    public static WithdrawalSellerData.WithDrawalDataTable GetAllWithdrawal()
    {
        
        WithdrawalSellerData.WithDrawalDataTable dt = new WithdrawalSellerData.WithDrawalDataTable();
        WithdrawalSellerDataTableAdapters.WithDrawalTableAdapter ta = new WithdrawalSellerDataTableAdapters.WithDrawalTableAdapter();


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

    public static WithdrawalSellerData.uspInqueryWithdrawalSellerDataTable GetWithdrawalByBusinessDate (Nullable<DateTime> businessDate)
    {
        WithdrawalSellerData.uspInqueryWithdrawalSellerDataTable dt = new WithdrawalSellerData.uspInqueryWithdrawalSellerDataTable();
        WithdrawalSellerDataTableAdapters.uspInqueryWithdrawalSellerTableAdapter ta = new WithdrawalSellerDataTableAdapters.uspInqueryWithdrawalSellerTableAdapter();

        try
        {
            ta.Fill(dt, businessDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
        
    #endregion

    

    

    
    
}
