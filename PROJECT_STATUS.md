# Intelligent Career Management System (ICMS)

## Project Information

**Project Name:** Intelligent Career Management System (ICMS)

### Backend

* ASP.NET Core 8 Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* MailKit SMTP

### Frontend

* Angular 21
* Bootstrap 5

### GitHub Repository

https://github.com/02ishan/intelligent-career-management-system

---

# Current Project Status

## Overall Progress

* Backend: 99%
* Frontend: 95%
* Overall Project: 97-98%

---

# Backend Modules

## Authentication & Authorization

Status: Completed ✅

Features:

* JWT Authentication
* Role-Based Authorization
* Student Role
* Recruiter Role
* Admin Role
* Secure Login
* Secure Registration

---

## Jobs Module

Status: Completed ✅

Features:

* Create Job
* View Jobs
* Update Job
* Delete Job
* Recruiter Ownership Validation
* Role-Based Access Control

---

## Applications Module

Status: Completed ✅

Features:

* Apply For Job
* View Applications
* View My Applications
* Duplicate Application Prevention
* Application Status Tracking
* Recruiter-Specific Application Filtering
* Dashboard-Based Filtering
* Status Updates

Application Statuses:

* Applied
* Under Review
* Shortlisted
* Interview Scheduled
* Selected
* Rejected

---

## Resume Module

Status: Completed ✅

Features:

* Upload Resume
* Download Resume
* Resume Storage

---

## ATS Engine

Status: Completed ✅

Features:

* Resume Parsing
* ATS Score Calculation
* Skill Matching
* Missing Skills Detection
* Matched Skills Detection

Example Response:

```json
{
  "applicationId": 4,
  "matchedSkills": [".NET"],
  "missingSkills": ["Angular"],
  "score": 50
}
```

---

## Email Notifications

Status: Completed ✅

Features:

* SMTP Integration
* Application Status Change Emails
* Test Email Endpoint

Note:

* Gmail App Password configuration may be required on local setup.

---

## Dashboard APIs

Status: Completed ✅

Features:

* Dashboard Statistics
* Total Jobs
* Total Applications
* Shortlisted Count
* Selected Count
* Rejected Count

---

# Frontend Status

## Setup

Status: Completed ✅

Features:

* Angular 21 Setup
* Bootstrap Integration
* Routing
* Services
* Guards
* Folder Structure

---

## Authentication

### Login Page

Status: Completed ✅

Features:

* API Integration
* JWT Storage
* Authentication Flow
* Navigation

### Register Page

Status: Completed ✅

Features:

* Registration Flow
* API Integration

---

## Dashboard

Status: Completed ✅

Features:

* Statistics Cards
* Dashboard Navigation
* Application Filters

---

## Jobs Module UI

Status: Completed ✅

Features:

* View Jobs
* Create Job
* Edit Job
* Delete Job
* Apply For Job
* Role-Based Button Visibility

Student View:

* Apply Button

Recruiter/Admin View:

* Edit Button
* Delete Button

---

## Applications Module UI

Status: Completed ✅

Features:

* Recruiter Applications View
* Status Updates
* Dashboard Integration
* Status-Based Filtering

---

## My Applications

Status: Completed ✅

Features:

* Student Application Tracking
* Application Status Visibility

---

## Navigation

Status: Completed ✅

Features:

* Reusable Navbar Component
* Role-Based Navigation
* Logout Functionality

Student Navigation:

* Jobs
* My Applications

Recruiter Navigation:

* Dashboard
* Jobs
* Create Job
* Applications

Admin Navigation:

* Dashboard
* Jobs
* Applications

---

# Test Accounts

## Student

Email:
[ishan@gmail.com](mailto:ishan@gmail.com)

Role:
Student

---

## Recruiter

Email:
[recruiter@gmail.com](mailto:recruiter@gmail.com)

Role:
Recruiter

---

## Admin

Email:
[admin@gmail.com](mailto:admin@gmail.com)

Role:
Admin

---

# Important Commits

### ATS Engine

Commit:
8298351

---

### Angular Frontend Initialization

Commit:
c0bf11d

---

### Authentication Architecture

Commit:
0886bb8

---

### Jobs CRUD Module

Commit:
a2c859a

---

### Applications & Navigation Module

Commit:
e1658b7

---

# Remaining Tasks

## Coding

* Hide Navbar on Login/Register Pages
* Final End-to-End Testing
* Minor UI Polish

Estimated Remaining Coding:
Less than 1 Hour

---

## Documentation

Pending:

* Project Report
* PPT
* Screenshots
* Database Schema Documentation
* Viva Preparation

---

# Target

Target Completion Date:
10 June 2026

Current Confidence:
Very High

Estimated Coding Completion:
Today

Estimated Project Completion:
Before 10 June 2026
