﻿// Generated by Selenium IDE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;


namespace ProjectTime_UITest.UserInterfaceTestcases
{ 
    [TestFixture]
    public class AdminUserUITest
    {
        private IWebDriver driver;
        public IDictionary<string, object> Vars { get; private set; }
        private IJavaScriptExecutor js;
        

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            Vars = new Dictionary<string, object>();
            driver.Navigate().GoToUrl("https://projecttime-app.azurewebsites.net/");
            driver.Manage().Window.Size = new System.Drawing.Size(1600, 900);
        }

        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        // UI Admin User test case begins by logging in as an admin user and logging of the application

        [Test, Order(1)]
        public void Test_AdminLogIn_LogOut()
        {
            
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();

        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Department,
        // it clicks on the add new department button and inputs required values and clicks on the create Button and completes

        [Test, Order(2)]
        public void Test_DepartmentCreate() 
        {
            
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Department")).Click();
            driver.FindElement(By.LinkText("Create New Department")).Click();
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("X Test Department");
            driver.FindElement(By.Id("Rate")).Click();
            driver.FindElement(By.Id("Rate")).SendKeys("25");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Department,
        // it clicks on the edit department button and inputs required values and clicks on the Update Button and completes

        [Test, Order(3)]
        public void Test_DepartmentEdit()
        {
            
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Department")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-primary"))).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("Rate")));
            driver.FindElement(By.Id("Rate")).Clear();
            driver.FindElement(By.Id("Rate")).SendKeys("35");
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("X Test Department 1 ");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Department,
        // it clicks on the delete department button, then delete button, it accepts the alert and completes  

        [Test, Order(4)]
        public void Test_DepartmentDelete()
        {
            
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Department")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-danger"))).Click();
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want to Delete this?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();

        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Non-Working Days,
        // it clicks on the add Non-Working days, inputs required values and clicks on the create Button and completes

        [Test, Order(5)]
        public void Test_NWDsCreate()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Non-Working Days")).Click();
            driver.FindElement(By.LinkText("Add Non-Working Day")).Click();
            driver.FindElement(By.Id("Date")).Click();
            driver.FindElement(By.Id("Date")).SendKeys("25-12-2022");
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).SendKeys("Christmas Days");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Non-Working Days,
        // it clicks on the edit Non-Working Days, button and inputs required values and clicks on the Update Button and completes

        [Test, Order(6)]
        public void Test_NWDsEdit()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Non-Working Days")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd .bi-pencil-square"))).Click();
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("Christmas Day");
            driver.FindElement(By.CssSelector("html")).Click();
            driver.FindElement(By.Id("AllowTimeLog")).Click();
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Non-Working Days,
        // it clicks on the delete Non-Working Days button, then delete button, it accepts the alert and completes  

        [Test, Order(7)]
        public void Test_NWDsDelete()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Non-Working Days")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd .btn-danger"))).Click();
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want to Delete this?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Project Stage,
        // it clicks on the add Project Stage button, inputs required values and clicks on the create Button and completes

        [Test, Order(8)]
        public void Test_CreateProjectStage()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project Stage")).Click();
            driver.FindElement(By.LinkText("Add project Stage")).Click();
            driver.FindElement(By.Id("Stage")).Click();
            driver.FindElement(By.Id("Stage")).SendKeys("X Test Project Stage");
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).SendKeys("X Test description");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Project Stage,
        // it clicks on the edit Project Stage button, inputs required values and clicks on the Update Button and completes

        [Test, Order(9)]
        public void Test_EditProjectStage()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project Stage")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-primary"))).Click();
            driver.FindElement(By.Id("Stage")).Click();
            driver.FindElement(By.Id("Stage")).Clear();
            driver.FindElement(By.Id("Stage")).SendKeys("X Test Edit Project Stage");
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("X Test Edit description");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Project Stage,
        // it clicks on the delete Project Stage button, then delete button, it accepts the alert and completes 

        [Test, Order(10)]
        public void Test_DeleteProjectStage()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project Stage")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-danger > .bi"))).Click();
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want to Delete this?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Project,
        // it clicks on the add Project button, inputs required values and clicks on the create Button and completes

        [Test, Order(11)]
        public void Test_CreateProject()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project")).Click();
            driver.FindElement(By.LinkText("Create New Project")).Click();
            driver.FindElement(By.Id("ProjectCode")).Click();
            driver.FindElement(By.Id("ProjectCode")).SendKeys("UTM005");
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("XX Test Project");
            driver.FindElement(By.Id("ProjectStageId")).Click();
            driver.FindElement(By.Name("ProjectStageId")).SendKeys("Initiation" + Keys.Enter);
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).SendKeys("Test project");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Project,
        // it clicks on the edit Project button, inputs required values and clicks on the Update Button and completes

        [Test, Order(12)]
        public void Test_EditProject()
        {

            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-primary"))).Click();
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("XX Test Project Edit");
            driver.FindElement(By.Id("ProjectStageId")).Click();
            driver.FindElement(By.Name("ProjectStageId")).SendKeys("Planning" + Keys.Enter);
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("Test project edit");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Project,
        // it clicks on the delete Project button, then delete button, it accepts the alert and completes 

        [Test, Order(13)]
        public void Test_DeleteProject()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .bi-trash-fill"))).Click();
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want to Delete this?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();

        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and Register User (With Role),
        // inputs required values and clicks on the register Button and completes

        [Test, Order(14)]
        public void Test_CreateUserWithRole()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Register User (with Role)")).Click();
            driver.FindElement(By.Id("Input_FullName")).Click();
            driver.FindElement(By.Id("Input_FullName")).SendKeys("XXX User");
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("xxxuser@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_ConfirmPassword")).Click();
            driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys("Coding@1234!");
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("Input_DepartmentId"))).Click();
            driver.FindElement(By.Id("Input_DepartmentId")).SendKeys("Analysis" + Keys.Enter);
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("Input_Role"))).Click();
            driver.FindElement(By.Id("Input_Role")).SendKeys("User" + Keys.Enter);
            driver.FindElement(By.Id("registerSubmit")).Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("xxxuser@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Manage Users,
        // it clicks on the edit user button, inputs required values and clicks on the Update Button and completes

        [Test, Order(15)]
        public void Test_EditUser()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Manage Users")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .bi-pencil-square"))).Click();
            driver.FindElement(By.Id("Email")).Click();
            driver.FindElement(By.Id("Email")).Click();
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("xxxuserEdit@projecttime.ie");
            driver.FindElement(By.Id("FullName")).Click();
            driver.FindElement(By.Id("FullName")).Clear();
            driver.FindElement(By.Id("FullName")).SendKeys("XXX User Edit");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Manage Users,
        // it clicks on the edit user button, then Manage Roles button, unselects user role and selects project manager role and clicks on the
        // Update Button and navigates to the logout button and logs out the admin user. it logs in as the user with the updated role and
        // logs out again  

        [Test, Order(16)]
        public void Test_EditUserRole()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Manage Users")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .bi-pencil-square"))).Click();
            driver.FindElement(By.LinkText("Manage Roles")).Click();
            driver.FindElement(By.Id("z2__IsSelected")).Click();
            driver.FindElement(By.Id("z0__IsSelected")).Click();
            driver.FindElement(By.Id("checkBtn")).Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("xxxuser@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".btn-link"))).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects Manage User
        // clicks on Delete/Lock button for User xxxuser@projecttime.ie, navigate and clicks on the Lock Button to lock out the user,
        // and navigates to the logout button and logs out the admin user. It then tries to login as xxxuser@projecttime.ie unsuccessfully,
        // it re-logs in as an Admin user, navigates to Content Management drop-down and selects Manage User
        // clicks on Delete/Lock button for User xxxuser@projecttime.ie, navigate and clicks on the Unlock Button to unlock the user,
        // and navigates to the logout button and logs out the admin user. It then successfully logs in as xxxuser@projecttime.ie and logs out

        [Test, Order(17)]
        public void Test_LockUnLockUserAccount()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Manage Users")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-danger"))).Click();
            driver.FindElement(By.LinkText("Lock/Unlock")).Click();
            driver.FindElement(By.CssSelector(".btn-success")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want Lock this user?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("xxxuser@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("ProjectTime")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Manage Users")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-danger"))).Click();
            driver.FindElement(By.LinkText("Lock/Unlock")).Click();
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want to Unlock this user?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("xxxuser@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.CssSelector(".btn-link")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects ProjectUser,
        // it clicks on the add ProjectUser button, inputs required values and clicks on the create Button and completes

        [Test, Order(18)]
        public void Test_CreateProjectUser()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project User")).Click();
            driver.FindElement(By.LinkText("Assign Project to User")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("ProjectId"))).Click();
            driver.FindElement(By.Id("ProjectId")).SendKeys("Turin" + Keys.Enter);
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("UserId"))).Click();
            driver.FindElement(By.Id("UserId")).SendKeys("XXX User Edit" + Keys.Enter);
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects ProjectUser,
        // it clicks on the edit ProjectUser button, inputs required values and clicks on the Update Button and completes

        [Test, Order(19)]
        public void Test_EditProjectUser()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project User")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-primary > .bi"))).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("IsActive"))).Click();
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI Admin User test case begins by logging in as an admin user, navigates to Content Management drop-down and selects ProjectUser,
        // it clicks on the delete ProjectUser button, then delete button, it accepts the alert and completes 

        [Test, Order(20)]
        public void Test_DeleteProjectUser()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("Admin@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Content Management")).Click();
            driver.FindElement(By.LinkText("Project User")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".odd:nth-child(5) .btn-danger > .bi"))).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want to Delete this?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();

        }


    }
}