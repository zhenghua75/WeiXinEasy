using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Mozart
{
    public class Global : System.Web.HttpApplication
    {
        IScheduler scheduler = null;
        protected void Application_Start(object sender, EventArgs e)
        {
            //创建并启动调度器
            //ISchedulerFactory factory = new StdSchedulerFactory();
            //scheduler = factory.GetScheduler();
            //创建任务
            //JobDetailImpl job = new JobDetailImpl();
            //JobDetail job = null;// new JobDetail("MyJob", typeof(ShiftNotify));
            ////创建触发器
            //CronTriggerImpl trigger = new CronTriggerImpl(); 
            //Trigger trigger = TriggerUtils.MakeMinutelyTrigger(5);
            //trigger.StartTimeUtc = TriggerUtils.GetEvenMinuteDate(DateTime.UtcNow);
            //trigger.Name = "ShiftNotify";
            //将任务与触发器添加到调度器中
            //scheduler.ScheduleJob(job, trigger);
            //scheduler.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            scheduler.Shutdown();
        }
    }
}