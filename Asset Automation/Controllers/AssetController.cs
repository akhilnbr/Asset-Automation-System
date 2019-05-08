using Asset_Automation.Models;
using Asset_Automation.ViewModel;
using Asset_Automation.Views.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Asset_Automation.Controllers
{
    public class AssetController : Controller
    {
        // GET: Asset
       private DbModel db;
        public AssetController()
        {
            db = new DbModel();
        }
        public ActionResult Index()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Index(register reg)
        {

            if (reg.check.Equals("admin") && reg.pin.Equals(1111))
            {
              
                return RedirectToAction("AddAsset");
            }

            ViewBag.alert = "Hmm,not a valid user";


            //reg = db.regmodel.Where(x => x.phone.Equals(reg.check)&&x.pin.Equals(reg.pin)||x.email.Equals(reg.check)&&x.pin.Equals(reg.pin)).SingleOrDefault();
            //if (reg != null)
            //{
            //    return RedirectToAction("Book");
            //}
            //else
            //{
            //    ViewBag.message = "Hmm,Seems like your not registered";
            //}

            return View();

        }

        [HttpGet]
        public ActionResult AddAsset()
        {

            var model = db.addasset.ToList();
        
            return View(model);
        }
        [HttpPost]
        public ActionResult AddAsset(Addasset add)
        {
            if(ModelState.IsValid)
            {
                var items = db.addasset.Where(x => x.asset.Equals(add.asset)).SingleOrDefault();
                if (items == null)
                {
                    add.state = "Active";
                  
                    //int a = add.capacity;
                    //add.assetid = "s" + a;
                   //for(int i=1;i<=a;i++)
                    //{
                        //add.capacity = i;
                        db.addasset.Add(add);
                        db.SaveChanges();
                    //}
                    

                }
                else
                {
                    ViewBag.message = "Following asset is already added";
                }

            }
           
    
           
            var model = db.addasset.ToList();
           
            return View(model);
        }
        public ActionResult Bookings(int? page)
        {

            int pagesize = 5, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var count = db.bookm.Where(x => x.status == "pending").Select(x => x.id).Count();
            TempData["count"] = Convert.ToInt32(count);
            var model = db.bookm.OrderByDescending(x => x.date).ToList();
            IPagedList<BookModel> list = model.ToPagedList(pageindex, pagesize);
            return View(list);

        }
        [HttpPost]
        public ActionResult Bookings(string selected)
        {
            var count = db.bookm.Where(x => x.status == "pending").Select(x => x.id).Count();
            TempData["count"] = Convert.ToInt32(count);
            selected = Request["date"].ToString();
            TempData["selected"] = selected;
            IPagedList<BookModel> model = db.bookm.Where(x => x.date == selected).ToPagedList();
            if(model!=null)
            {
                return View(model);
                
            }
            ViewBag.bookings = "No entries for the selected date";
            return View();
           
        }
   
        public ActionResult Approve(int id )
        {
            var count = db.bookm.Where(x => x.status == "pending").Select(x => x.id).Count();
            TempData["count"] = Convert.ToInt32(count);
            var item = db.bookm.Where(x => x.id == id).First();
            item.status = "Approved";          
            try
            {


                var sendmail = new MailAddress("enter your valid email address", "Akhil");
                var receiver = new MailAddress(item.email, "Buddy");
                var password = "";//enter your email password
                var subject = "Confirmation Mail";
                var body = "This is to confirm that the asset" + item.asset + "is APPROVED for the date" + item.date + "from" + item.intime + "to" + item.outtime;




                var smptp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(sendmail.Address, password)
                };
                using (var mess = new MailMessage(sendmail, receiver)
                {
                    Subject = subject,
                    Body = body,


                })
                {
                    smptp.Send(mess);
                }

                ViewBag.Status = "Please check your mail";
          

            }
            catch
            {
                ViewBag.Status = "Problem while sending email, Please check details.";
            }

            var items = db.bookm.ToList();
            db.SaveChanges();







            return View("Bookings", items);
        }



        public enum Alert
        {
            success,
            failure
        }
  


        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(register reg)
        {
            if (ModelState.IsValid)
            {
                var regs = db.regmodel.Where(x => x.email.Equals(reg.email)).Count();
                var emails = db.regmodel.Where(x => x.phone.Equals(reg.phone)).Count();
                if (regs==0&&emails==0)
                {

                  
                    db.regmodel.Add(reg);
                    db.SaveChanges();
                    ViewBag.suucesss = "Succesfully";

                    return RedirectToAction("Index");

                }
                ModelState.Clear();
                ViewBag.sorry = "Registered User!!!";
            }
           
           
               


            return View();
           
            
        }
       


        [HttpGet]
        public  ActionResult Book(string str="")
        {
            BookinView bv = new BookinView();



            List<Addasset> assetlist = db.addasset.ToList();
            ViewBag.list = new SelectList(assetlist, "asset", "asset");
            return View(bv);
           

        }


       


        [HttpPost]
        public ActionResult Book(BookModel model,register reg)
        {

            List<Addasset> assetlist = db.addasset.ToList();
            ViewBag.list = new SelectList(assetlist, "asset", "asset");
            string date = Request["date"];
            string asset = Request["asset"];
           
            TimeSpan itime = TimeSpan.Parse(Request["intime"]);
            TimeSpan otime = TimeSpan.Parse(Request["outtime"]);
                var items = db.bookm.Where(x => x.date == date && x.intime >= itime && x.outtime <= otime && x.asset == asset).SingleOrDefault();
                if (items == null)
                {


                    var regs = db.regmodel.Where(x => x.phone.Equals(reg.check) && x.pin.Equals(reg.pin) || x.email.Equals(reg.check) && x.pin.Equals(reg.pin)).SingleOrDefault();
                    if (regs != null)
                    {







                        var email = (from a in db.regmodel
                                     where a.phone.Equals(reg.check) || a.email.Equals(reg.check)
                                     select new
                                     {

                                         a.email,
                                         a.name,
                                         a.phone


                                     }).SingleOrDefault();
                        string Email = email.email;
                       
                        model.name = email.name;
                        model.phone = email.phone;
                        model.email = email.email;
                        db.bookm.Add(model);
                        
                        model.status = "pending";
                        db.bookm.Add(model);
                        db.SaveChanges();



                        try
                        {


                            var sendmail = new MailAddress("enter your valid email address", "Akhil");
                            var receiver = new MailAddress(Email, "Buddy");
                            var password = "";//enter your email password
                            var subject = "Request for Asset!!";
                            var body = "This is regarding the request by " + model.name + " for the " + model.asset + "on " + date + "from" + itime + "to" + otime;




                            var smptp = new SmtpClient
                            {
                                Host = "smtp.gmail.com",
                                Port = 587,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = false,
                                Credentials = new NetworkCredential(sendmail.Address, password)
                            };
                            using (var mess = new MailMessage(sendmail, receiver)
                            {
                                Subject = subject,
                                Body = body,


                            })
                            {
                                smptp.Send(mess);
                            }
                            TempData["status"] = "Please keep on checking your mail.Booking is Confirmed for  " + date + " and " + itime + " and " + otime + "";

                            return RedirectToAction("Index");

                        }
                        catch
                        {
                            TempData["probblem"] = "Problem while sending email, Please check details.";
                        }





                       




                    }
                    else
                    {

                        ViewBag.notauser = "Not a valid user";
                    RedirectToAction("Book");
                    }



                }
            else
            {

                BookinView bv = new BookinView
                {
                    book = db.bookm.Where(x => x.date == date && x.intime >= itime && x.outtime <= otime).ToList()



                };
                return View(bv);
            }




            return View();
        }




        public ActionResult ExportToExcel()
        {

            try
            {

                var dates = TempData["selected"].ToString();
                var data = (from a in db.bookm where a.date==dates

                            select new
                            {
                                a.name,
                                a.department,
                                a.asset,
                                a.date,
                                a.intime,
                                a.outtime

                            }).ToList();
                if (data != null)
                {
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename =Export-'" + dates + "'.xls");

                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    Response.Buffer = true;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {

                            gridview.RenderControl(htw);
                            Response.Output.Write(sw.ToString());
                            Response.Flush();
                            Response.End();
                        }
                    }



                }


            }
            catch
            {
                ViewBag.message = "Please select the date";

            }

            return View();

        }


        public ActionResult Active(int id,Addasset bm)
        {
            var items = db.addasset.Where(x => x.id == id).FirstOrDefault();
            items.state = "Active";
            db.SaveChanges();

            var updation = db.addasset.ToList();
            return View("AddAsset", updation); 
        }


        public ActionResult InActive(int id)
        {
            var items = db.addasset.Where(x => x.id == id).FirstOrDefault();
            items.state = "InActive";

            db.SaveChanges();
            var updation = db.addasset.ToList();

            return View("AddAsset", updation);
        }
        public ActionResult DeleteAsset(int id,Addasset asset,BookModel model)
        {

             asset = db.addasset.Find(id);
            try
            {

                var check = (from a in db.bookm
                             where a.asset == asset.asset
                             select new
                             {
                                 a.date

                             }).SingleOrDefault();


                DateTime booked_date = Convert.ToDateTime(check.date);
                if (booked_date <= DateTime.Today.Date && check == null)
                {
                    db.addasset.Remove(asset);
                    db.SaveChanges();

                }
                else {

                    ViewBag.booked = "Items are booked";

                }

            }
            catch
            {
                db.addasset.Remove(asset);
                db.SaveChanges();
            }
           
           

       
            var updation = db.addasset.ToList();

            return View("AddAsset", updation);
        }
        [HttpGet]
        public ActionResult Checkavailablity()
            {
            List<Addasset> assetlist = db.addasset.ToList();
            ViewBag.list = new SelectList(assetlist, "asset", "asset");

            BookinView bv = new BookinView();
           
            return View(bv);
        }
        [HttpPost]
        public ActionResult Checkavailablity(BookModel model)
        {
            List<Addasset> assetlist = db.addasset.ToList();
            ViewBag.list = new SelectList(assetlist, "asset", "asset");
            if (ModelState.IsValid)
            {
                string dept = Request["department"];
                string date = Request["date"];
                string members = Request["members"];
                string asset = model.asset;
                TimeSpan itime = TimeSpan.Parse(Request["intime"]);
                TimeSpan otime = TimeSpan.Parse(Request["outtime"]);
             
                    var list = db.bookm.Where(x => x.date == date && x.intime >= itime && x.outtime <= otime && x.asset.Equals(model.asset)).ToList();
                    if (list.Count == 0)
                    {
                        TempData["avaiable"] = asset+ " are available for booking for the date " + date + " and " + itime + " and " + otime + "";
                    Session["dept"] = dept;
                    Session["asset"] = asset;
                    Session["date"] = date;
                    Session["itime"] = itime;
                    Session["otime"] = otime;
                    Session["asset"] = model.asset;
                    Session["members"] = Request["members"];


                        return RedirectToAction("Book");
                    }
                    else
                    {
                        BookinView bv = new BookinView
                        {
                            book = db.bookm.Where(x => x.date == date && x.intime >= itime && x.outtime <= otime).ToList()



                        };

                        return View(bv);
                    }

                }


       
            return View();
            


            
            
        }
        public ActionResult Forgotpassword(string email)
        {

            TempData["email"] = email;


            var reset = db.regmodel.Where(x => x.email == email).SingleOrDefault();
            if (reset == null)
            {
                TempData["Sorry"] = "Sorry you are not registred user";
            }
            else {
                try
                {


                    var sendmail = new MailAddress("enter your email address", "Akhil");
                    var receiver = new MailAddress(email, "Buddy");
                    var password = "";//enter your email password
                    var subject = "Reset Pin";
                    var body = "Please follow the link  to reset the PIN \"http://localhost:54068/Asset/Resetpasword\" ";




                    var smptp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(sendmail.Address, password)
                    };
                    using (var mess = new MailMessage(sendmail, receiver)
                    {
                        Subject = subject,
                        Body = body,


                    })
                    {
                        smptp.Send(mess);
                    }
                    TempData["statusemail"] = "Reset link is send to email.Please";

                    return RedirectToAction("Book");

                }
                catch
                {
                    TempData["probblem"] = "Problem while sending email, Please check details.";
                }




            }

            return RedirectToAction("Book",new { ViewBag.message });
        }



        public ActionResult Resetpasword()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Resetpasword( reset r ) {


          
        
            try
            {
                string email = TempData["email"].ToString();
                var update = db.regmodel.Single(x => x.email == email);
                update.pin = r.pin;
                db.SaveChanges();
                TempData["updated pin"] = "your pin has been successfully updated.Please continue with your Booking";
                return RedirectToAction("Book");
               

               
            }
            catch {
                ViewBag.message("link is expired");
            }


            return RedirectToAction("Book");






        }

        public JsonResult Getetails(string date)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<BookModel> collect = db.bookm.Where(x => x.date == date).ToList();
            return Json(collect, JsonRequestBehavior.AllowGet);

        }




        public JsonResult Getmembers(string asset)
        {
            db.Configuration.ProxyCreationEnabled = false;
            
            List<Addasset> collection = db.addasset.Where(x => x.asset == asset).ToList();
            return Json(collection, JsonRequestBehavior.AllowGet);

        }


        //public FileResult Export(string ExportData)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        var dates = TempData["selected"].ToString();
        //        var data = (from a in db.bookm
        //                    where a.date == dates

        //                    select new
        //                    {
        //                        a.name,
        //                        a.department,
        //                        a.asset,
        //                        a.date,
        //                        a.intime,
        //                        a.outtime

        //                    }).ToList();
        //        StringReader reader = new StringReader(data.ToString());
        //        Document PdfFile = new Document(PageSize.A4);
        //        PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
        //        PdfFile.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
        //        PdfFile.Close();
        //        return File(stream.ToArray(), "application/pdf", "ExportData.pdf");
        //    }
        //}



    }




}
