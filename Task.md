# SoftwareMind-Intern-Challenge

## Description

A hot desk booking system is a system which should be designed to automate the reservation of desks in an office through an easy online booking system.

## Requirements

Administration:
- Manage locations (add/remove, can't remove if desk exists in location)
- Manage desk in locations (add/remove, if no reservation/make unavailable)

Employees:
- Determine which desks are available to book or unavailable.
- Filter desks based on location
- Book a desk for the day.
- Allow reserving a desk for multiple days but no more than a week.
- Allow to change desk, but not later than the 24h before reservation.
- Administrators can see who reserves a desk in location, whereas Employees can see only that specific desk is unavailable.

Start with API but if you feel you can also do the frontend 'go for it'
API must be written in .Net
Create at least one unit test
There are no restrictions in mythology/libraries etc
