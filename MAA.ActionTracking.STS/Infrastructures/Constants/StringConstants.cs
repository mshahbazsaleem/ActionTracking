using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures.Constants
{
    public class StringConstants
    {
        public const string Saml2 = "SAML2.0";

        public const string EMAIL_ACTIVATION_TEMPLATE = "Activation";
        public const string EMAIL_STANDARD_HEADER = "Standard Header";
        public const string EMAIL_STANDARD_FOOTER = "Standard Footer";

        /***************************************************************/
        //                   App Settings                              //
        /***************************************************************/
        public const string IdentityServerHost = "IdentityServerHost";
        public const string ApplicationTitle = "ApplicationTitle";
        public const string CopyrightText = "CopyrightText";
        public const string SendGridAPIKey = "SendGridAPIKey";
        public const string FromEmail = "FromEmail";
        public const string FromName = "FromName";
        public const string DarticanDomain = "DarticanDomain";


        /***************************************************************/
        //                   Email Template Placeholders               //
        /***************************************************************/
        public const string PLACEHOLDER_TEMPLATE_SUBJECT = "%Template Subject%";
        public const string PLACEHOLDER_TEMPLATE_SENDER = "%Template Sender%";
        public const string PLACEHOLDER_TEMPLATE_FROM_NAME = "%Template From Name%";
        public const string PLACEHOLDER_TEMPLATE_FROM_EMAIL_ADDRESS = "%Template From Email Address%";
        public const string PLACEHOLDER_DM_FIRST_NAME = "%DM First Name%";
        public const string PLACEHOLDER_DM_LAST_NAME = "%DM Last Name%";
        public const string PLACEHOLDER_DM_FULL_NAME = "%DM Full Name%";
        public const string PLACEHOLDER_DM_SUPERIOR = "%DM Superior%";
        public const string PLACEHOLDER_DM_DIRECT_REPORTS = "%DM Direct Reports%";
        public const string PLACEHOLDER_DM_LOGIN_IDENTIFIER = "%DM Login Identifier%";
        public const string PLACEHOLDER_DM_EMAIL_ADDRESS = "%DM Email Address%";
        public const string PLACEHOLDER_DM_LAST_LOGIN_DATE_TIME = "%DM Last Login Date &amp; Time%";
        public const string PLACEHOLDER_DM_LAST_LOGOUT_DATE_TIME = "%DM Last Logout Date &amp; Time%";
        public const string PLACEHOLDER_DM_ACTIVATION_LINK = "%DM Activation Link%";
        public const string PLACEHOLDER_DM_EXCEL_WORKBOOK_PASSWORD = "%DM Excel Workbook Password%";
        public const string PLACEHOLDER_DM_EXCEL_HR_REP_PASSWORD = "%DM Excel HR Rep Password%";
        public const string PLACEHOLDER_DM_EXCEL_AREA_HR_REP_PASSWORD = "%DM Excel Area HR Rep Password%";
        public const string PLACEHOLDER_EMPLOYEE_IDENTIFIER = "%Employee Identifier%";
        public const string PLACEHOLDER_EMPLOYEE_NAME = "%Employee Name%";
        public const string PLACEHOLDER_EMPLOYEE_JOB_TITLE = "%Employee Job Title%";
        public const string PLACEHOLDER_EMPLOYEE_POSITION_TITLE = "%Employee Position Title%";
        public const string PLACEHOLDER_EMPLOYEE_SALARY_PLAN_SUBMITTED = "%Employee Salary Plan Submitted%";
        public const string PLACEHOLDER_EMPLOYEE_BONUS_PLAN_SUBMITTED = "%Employee Bonus Plan Submitted%";
        public const string PLACEHOLDER_EMPLOYEE_LTI_PLAN_SUBMITTED = "%Employee LTI Plan Submitted%";
        public const string PLACEHOLDER_APPLICATION_LOGO = "%Application LOGO%";
        public const string PLACEHOLDER_APPLICATON_TITLE = "%Application Title%";
        public const string PLACEHOLDER_APPLICATION_DATE_TIME = "%Application Date &amp; Time%";
        public const string PLACEHOLDER_APPLICATION_DATE = "%Application Date%";
        public const string PLACEHOLDER_APPLICATION_FOOTER_HELP_TEXT = "%Application Footer Help Text%";
        public const string PLACEHOLDER_APPLICATION_FOOTER_EMAIL = "%Application Footer Email%";



    }
}
