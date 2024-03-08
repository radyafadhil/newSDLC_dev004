using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using FormsAuth;
using new_SDLC.Models;

namespace new_SDLC.Controllers
{
    
    public class HomeController : Controller
    {

        GeneralClass gc = new GeneralClass();
        db_sdlcDataContext dbSdlc = new db_sdlcDataContext(ConfigurationManager.ConnectionStrings["DB_ICT_eSDLCConnectionString"].ConnectionString);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AppList()
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                return View();
            }
        }

        public ActionResult AppDetail(string id)
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                Session["idApp"] = id;
                return View();
            }            
        }

        //================================== GET DATA

        public JsonResult Logout()
        {
            try
            {
                Session.RemoveAll();
                return Json(new { Status = true, Message = "Berhasil" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err Logout :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult LoginLDAP(ClsLogin clsLogin)
        {
            try
            {
                var ldap = new LdapAuthentication("LDAP://KPPMINING:389");
                //var abc = ldap.IsAuthenticated("KPPMINING", clsLogin.Username, clsLogin.Password);
                var abc = true;
                
                if (abc == true)
                {
                    var getName = dbSdlc.TBL_M_PICs.Where(x => x.NAME.ToUpper() == clsLogin.Username.ToUpper()).FirstOrDefault();
                    gc.unameGlobal = getName.PIC_DEV.ToUpper();

                    Session["kodename"] = getName.NAME.ToUpper();
                    Session["uname"] = getName.PIC_DEV.ToUpper();
                    FormsAuthentication.SetAuthCookie(clsLogin.Username, clsLogin.rememberMe);

                    return Json(new { Status = true, Message = "Berhasil" }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Gagal" }, JsonRequestBehavior.AllowGet);
                }
                
            }

            catch (Exception e)
            {
                return Json(new { Status = false, Message = "Err LoginLDAP :" + e.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult GetAppList()
        {
            try
            {
                var get = dbSdlc.cufn_getAllApps();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err GetReviewActivity : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getPlatform()
        {
            try
            {
                var get = dbSdlc.TBL_M_PLATFORMs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch ( Exception ex )
            {
                return Json(new { Status = false, Message = "Err getPlatform : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getServer()
        {
            try
            {
                var get = dbSdlc.TBL_M_SERVERs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getServer : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getStatus()
        {
            try
            {
                var get = dbSdlc.TBL_M_STATUS.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getStatus : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailApp()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.TBL_T_APPs.Where(x => x.APP_ID.ToString() == id.ToString().ToUpper()).FirstOrDefault();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getDetailApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        //================================== GET DATA

        public JsonResult InsertNewApp(ClsAppList AppList)
        {
            try
            {
                Guid newId = Guid.NewGuid();

                //insert app
                TBL_T_APP nd = new TBL_T_APP();                
                nd.APP_ID = newId;
                nd.APP_NAME = AppList.APP_NAME;
                nd.OWNER = AppList.OWNER;
                nd.PLATFORM_ID = AppList.PLATFORM.ToUpper();
                nd.SERVER_ID = AppList.SERVER.ToUpper();
                nd.STATUS_ID = AppList.STATUS.ToUpper();
                dbSdlc.TBL_T_APPs.InsertOnSubmit(nd);

                //insert version
                TBL_T_VERSION nv = new TBL_T_VERSION();
                nv.VERSION_ID = Guid.NewGuid();
                nv.DATE = DateTime.Now.Date;
                nv.VERSION = AppList.VERSION;
                nv.APP_ID = newId.ToString().ToUpper();
                dbSdlc.TBL_T_VERSIONs.InsertOnSubmit(nv);

                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Simpan ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteAppList(string id)
        {
            try
            {
                var delete = dbSdlc.TBL_T_APPs.Where(x => x.APP_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                dbSdlc.TBL_T_APPs.DeleteOnSubmit(delete);

                var delVersion = dbSdlc.TBL_T_VERSIONs.Where(x => x.APP_ID.ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                dbSdlc.TBL_T_VERSIONs.DeleteOnSubmit(delVersion);

                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Hapus ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getListDocument()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.cufn_getAllDocs().Where(x => x.APP_ID.ToUpper() == id.ToUpper()).ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getListVersion()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.cufn_getAllVersions().Where(x => x.APP_ID.ToUpper() == id.ToUpper()).ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getListDatabase()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.cufn_getAllDatabases().Where(x => x.APP_ID.ToUpper() == id.ToUpper()).ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }

    public class ClsLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool rememberMe { get; set; }
    }

    public class ClsAppList
    {
        public string APP_ID { get; set; }
        public string APP_NAME { get; set; }
        public string OWNER { get; set; }
        public string PLATFORM { get; set; }
        public string SERVER { get; set; }
        public string STATUS { get; set; }
        public string VERSION { get; set; }
    }

}