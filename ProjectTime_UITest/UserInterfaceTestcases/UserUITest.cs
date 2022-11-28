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
    public class UserUITest
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

        // UI User test case begins by logging in as a User role and logout of the application

        [Test, Order(1)]
        public void Test_UserLogIn_LogOut()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("user@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();

        }

        // UI User test case begins by logging in as a user, navigate and click on the Hello @Username link, navigate and click on the 
        // change password link input password values and click save

        [Test, Order(2)]
        public void Test_ChangePassword()
        {
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("user@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Hello user@projecttime.ie!")).Click();
            driver.FindElement(By.Id("change-password")).Click();
            driver.FindElement(By.Id("Input_OldPassword")).Click();
            driver.FindElement(By.Id("Input_OldPassword")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_NewPassword")).Click();
            driver.FindElement(By.Id("Input_NewPassword")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_ConfirmPassword")).Click();
            driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys("Coding@1234!");
            driver.FindElement(By.CssSelector(".w-100")).Click();
            driver.FindElement(By.CssSelector(".btn-link")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("user@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("login-submit")).Click();
        }

        // UI User test case begins by logging in as a user, clicks on the Add TimeLog button from the home page,
        //  and inputs required values and clicks on the create Button and completes

        [Test,Order(3)]
        public void Test_CreateTimelog()
        {

            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("ciaran.russell92@gmail.com");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("Log ProjectTime")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("ProjectId"))).Click();
            driver.FindElement(By.Id("ProjectId")).SendKeys("Rome" + Keys.Enter);
            driver.FindElement(By.Id("Date")).Click();
            driver.FindElement(By.Id("Date")).SendKeys("26-11-2022");
            driver.FindElement(By.Id("Duration")).Click();
            driver.FindElement(By.Id("Duration")).SendKeys("9");
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).SendKeys("Test Timelog");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }


        // UI User test case begins by logging in as a user, navigates to My Time Log's drop-down and selects Time Log's,
        // it clicks on the edit Time log button, in the itemised Time Log page it clicks on the edit button,
        // inputs required values and clicks on the Update Button and completes

        [Test, Order(4)]
        public void Test_EdidTimeLog()
        {

            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("ciaran.russell92@gmail.com");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("My Time Logs")).Click();
            driver.FindElement(By.LinkText("Time Log Summary")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".mx-2 > .bi"))).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit"))).Click();
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("Test Timelog Edit");
            driver.FindElement(By.Id("Duration")).Click();
            driver.FindElement(By.Id("Duration")).Clear();
            driver.FindElement(By.Id("Duration")).SendKeys("7.5");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        // UI User test case begins by logging in as a user, navigates to My Time Log's drop-down and selects Time Logs,
        // it clicks on the delete Time Log button, in the itemised Project estimate page it clicks on the delete button,
        // it accepts the alert and completes

        [Test, Order(5)]
        public void Test_DeleteTimeLog()
        {

            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("ciaran.russell92@gmail.com");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("My Time Logs")).Click();
            driver.FindElement(By.LinkText("Time Log Summary")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Time Log's"))).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".bi-trash-fill"))).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(driver.SwitchTo().Alert().Text, Is.EqualTo("Are you sure you want to Delete this?"));
            driver.SwitchTo().Alert().Accept();
            driver.SwitchTo().ActiveElement().Click();
        }

        // UI User test case begins by logging in as a User role and clicking on the About link

        [Test, Order(6)]
        public void Test_About()
        {

            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("user@projecttime.ie");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Coding@1234!");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.LinkText("About")).Click();
        }


    }
}