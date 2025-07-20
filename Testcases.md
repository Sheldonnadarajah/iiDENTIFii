# 🧪 Backend Software Engineer — Forum API Test Plan

This document outlines a suite of test cases for verifying the core functionality of the Forum API backend. The system is built with ASP.NET in C# and includes endpoints for handling posts, comments, likes, user authentication, and moderation.

---

## 📝 Post Functionality

- **Create Post**
  - ✅ Valid user submits a post with title and content  
  - ❌ Anonymous user attempts to submit a post  
  - ❌ Submission with missing required fields (e.g. no title)

- **Retrieve Posts**
  - ✅ Retrieve all posts with pagination  
  - ✅ Filter posts by date range, author, and tags  
  - ✅ Sort posts by creation date and number of likes

---

## 💬 Comment Functionality

- **Add Comment**
  - ✅ Logged-in user submits a comment on an existing post  
  - ❌ Anonymous user attempts to comment  
  - ❌ Commenting on a non-existent post

- **Retrieve Comments**
  - ✅ Fetch all comments for a given post  
  - ✅ Check for pagination behavior

---

## 👍 Like Functionality

- **Like a Post**
  - ✅ Valid user likes someone else's post  
  - ❌ User attempts to like their own post  
  - ❌ User likes the same post multiple times

- **Unlike a Post**
  - ✅ User unlikes a previously liked post  
  - ❌ User tries to unlike a post they never liked

---

## 🔐 Authentication & Authorization

- **Login Flow**
  - ✅ Successful login with correct credentials  
  - ❌ Login failure with incorrect password  
  - ❌ Unauthorized access to restricted endpoints

- **User Role Behavior**
  - ❌ Regular user attempts to tag post  
  - ✅ Moderator tags post successfully  
  - ❌ Moderator tries to tag a non-existent post

---

## 🛠️ API Integrity & Edge Cases

- **Abuse Handling**
  - ❌ Rapid-fire requests from same user to test rate limits  
  - ❌ Injection or malformed input sent to endpoints

- **Documentation Validation**
  - ✅ Each documented endpoint returns appropriate status codes  
  - ✅ Test Postman collection reflects actual API behavior

---

## 🧪 Notes

These tests can be implemented using:
- ✅ C# unit/integration tests (e.g., using xUnit or NUnit)  
- ✅ A Postman collection with test scripts and assertions

---