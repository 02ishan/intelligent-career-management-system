# Intelligent Career Management System (ICMS)

## Project Information

**Project Name:** Intelligent Career Management System (ICMS)

**Backend:**

* ASP.NET Core 8 Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* MailKit SMTP

**Frontend:**

* Angular 21
* Bootstrap 5

**GitHub Repository:**
https://github.com/02ishan/intelligent-career-management-system

---

# Current Project Status

## Overall Progress

* Backend: 98%
* Frontend: 25%
* Overall Project: ~80%

---

# Backend Modules

## Authentication & Authorization

Status: Completed

Features:

* JWT Authentication
* Role-Based Authorization
* Student Role
* Recruiter Role
* Admin Role

---

## Jobs Module

Status: Completed

Features:

* Create Job
* Update Job
* Delete Job
* Get Jobs
* Recruiter Ownership Validation

---

## Applications Module

Status: Completed

Features:

* Apply For Job
* View Applications
* Application Details DTO
* Status Updates
* Remarks

---

## Resume Module

Status: Completed

Features:

* Upload Resume
* Download Resume
* Resume Storage

Path Example:

Uploads/Resumes/1002_Ishan_Verma_CV.pdf

---

## ATS Engine

Status: Completed

Features:

* Resume Parsing
* Keyword Matching
* Skill Match API
* Missing Skills API
* ATS Score Calculation

Example Response:

{
"applicationId": 4,
"matchedSkills": [
".NET"
],
"missingSkills": [
"Angular"
],
"score": 50
}

---

## Email Notifications

Status: Completed

Features:

* SMTP Integration
* Test Email Endpoint
* Application Status Change Emails

Provider:

* Gmail SMTP

---

## Dashboard APIs

Status: Completed

Features:

* Dashboard Statistics
* Application Analytics

---

# Frontend Status

## Setup

Status: Completed

Features:

* Angular 21 Setup
* Bootstrap Integration
* Routing
* Folder Structure

---

## Folder Structure

src/app

components/

* login
* register

pages/

* login-page
* register-page

services/

* auth

guards/

* auth-guard

models/

shared/

---

## UI Progress

### Login Page

Status: In Progress

Completed:

* Routing
* Bootstrap Layout
* Professional UI Theme

Pending:

* Form Binding
* Validation
* API Integration
* JWT Storage

---

### Register Page

Status: Not Started

---

### Dashboard

Status: Not Started

---

### Navbar

Status: Not Started

---

# Test Accounts

## Student

Email:
[ishan@gmail.com](mailto:ishan@gmail.com)

Password:
(Use current local database password)

---

## Recruiter

Email:
[recruiter@gmail.com](mailto:recruiter@gmail.com)

Password:
(Use current local database password)

---

## Admin

Email:
[admin@gmail.com](mailto:admin@gmail.com)

Password:
(Use current local database password)

---

# Important Commits

Implement ATS resume scoring engine

Commit:
8298351

---

Initialize Angular frontend with Bootstrap setup

Commit:
c0bf11d

---

Create Angular frontend architecture and authentication scaffolding

Commit:
0886bb8

---

# Current Theme

Login Page Theme:

* Soft Professional Blue
* Modern SaaS Style
* Bootstrap Based
* Minimal UI

---

# Next Tasks

1. Complete Register Page UI
2. Login API Integration
3. Register API Integration
4. JWT Token Storage
5. Route Guard Implementation
6. Logout Functionality
7. Dashboard UI
8. Jobs UI
9. Applications UI
10. Final Testing

---

# Target

Target Completion Date:
10 June 2026

Current Confidence:
High
