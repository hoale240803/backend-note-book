# Interview preparation

1. CV
   Write a paragraph with 200 words with pattern CAR (context, action, result).

2. Algorithm

- min blind 75 problem in LeetCode. Or practice with Neetcode.

3. Behaviors

-

## CV

1. Flexton
   Context: I join with a Fresher role. I'm familiar with ReactJs, Dotnet core 3.1, SqlServer task is quite complex, it's take performance 30second to load, when i have to optimize the query to be load faster and don't let FE call continues to BE. While researching the meaning and mechanism each line of code. I guess the problem related data or re-rendering to many time. I check the console and saw that it's call api many time, each time api return thousand records and whole entity of that table.

Action:

I plan optimize both FE and BE. FE i debug and find the place trigger re-render multiple times. Once i fix the FE done. I find the root cause on the BE side. I debug and it return whole entities of data there are no pagination. I implement pagination, filter to reduce the data return for the first time with default filter, mapping only need data.

Result: It's loaded under 1s instead 30s for the first time enter.

2. Product Marketing:

Context: Implement a appointment feature integrate with AI booking with matching keyword. Allow user able to book manually with prospect or turn on the option "Auto booking with AI".

Action: Research job related ai job what how does it worked? how to sync data to calendar.

Result: After a week the user use it with over 1000 appointments.

3. Army form:
   Context: User want to reduce the cost of EC2 computing. switch to linux type to mitigate cost and dockernizer for easy to deploy

   Action: research and understanding the dockerize, nginx, mapping domain to switch new home.

   Result: Save cost 20% after deploy

4. GrowthMind:
   Context: Check the resource on Azure and turn of or remove unused service.
   Action: Check app service, Azure storage remove and clean up service unused the staging and dev to save cost.
   Result: Save 50% cost of operation.

5. Marketing: the 3rd bankrupted migration overall application to another provider. Then prepare for another one.

Action: Go th the source code, check impact the migration. Break down them into small milestone implement with small service first then bigger service later.

Result: Ending up before the deadline 1 month. Keep logic still working as normal.

## Improve writing

1. Frontend and Backend Optimization – Internal application
   As a fresher, I worked on a performance-critical application using ReactJS, .NET Core 3.1, and SQL Server. I identified a severe load time issue—pages took 30 seconds to render due to repeated API calls and backend responses returning entire datasets without pagination. I optimized the frontend to prevent unnecessary re-renders and updated the backend to return only paginated and filtered data. This reduced the load time to under 1 second and significantly improved the user experience.

2. AI-Powered Appointment Booking Feature – Product Marketing
   I implemented an appointment scheduling feature that supported both manual booking and automated AI-powered keyword matching. After researching how AI-driven job scheduling works and how to sync with calendars, I integrated the system into our existing platform. The feature was well-received, with over 1,000 appointments booked in its first week.

3. Cost Optimization for Cloud Infrastructure – Army Form
   To reduce EC2 operating costs, I migrated services from Windows to Linux-based instances and containerized the application using Docker. I also configured Nginx and mapped domains for deployment. This transition not only simplified deployment but also cut infrastructure costs by 20%.

4. Azure Resource Optimization – GrowthMind
   I audited our Azure environment to identify unused resources and remove redundant staging and development services. By cleaning up App Services and storage accounts, I was able to reduce operational costs by 50% without impacting production.

5. Application Migration to DSP Provider – Marketing
   After a third-party provider declared bankruptcy, I led the migration of our entire platform to a new cloud provider. I reviewed the codebase to assess potential impact, then designed a phased migration plan—starting with smaller services and scaling up. The migration was completed a month ahead of schedule, with full system functionality maintained throughout.
