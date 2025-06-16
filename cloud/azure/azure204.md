# 1. Develop Azure compute solutions

In this track of the az-204: Developing Solutions for Microsoft Azure journey, the focus will be on the following:

- Implement containerized solutions
- Implement Azure App Service Web Apps
- Implement Azure Functions

## 1.1 Virtual machines

### Discover the key concepts covered in this course

- Create and manage Azure VMs using a variety of tools and techniques, including the Azure CLI, Azure Portal, and Azure PowerShell

- Learn about the basics of Azure VMs and how to create and configure both Linux and Windows VMs in Azure.

- Discuss how to connect to your VMs using various methods, including SSH and Secure Shell(SSH) on Linux VMs and Remote Desktop Protocol(RDP) on Windows VMs.

- Cover how to use SSH public-private key pairs

### Outline the use of virtual machines (VMs) in Azure

- Virtualization/emulation of a physical computer system. E.g a digital version of a physical computer

- Runs on an isolated partition of its host computer with its own CPU, memory, network interface, and storage.

- Ideal when seeking more control over the computing environment

- Enables virtualization without having to purchase or maintain the physical hardware used to operate virtual machines

- Configuring, Patching, Installing

**Use cases**

- Development & testing
- Applications
- Datacenter

**Considerations**

- Resource locations
- Virtual machine sizes
- Maximum number of virtual machines
- Virtual machine configuration(s)

**Locations to create or access resources**

- Azure portal
- Azure PowerShell
- Azure CLI
- REST API

**Availability**

- Availability zones
- Virtual machine scale sets

**Pricing Factors**

- Processing power: the more cpu, thee more the cost.
- Memory: the same processing power
- Storage capacity: larger is more expensive
- Network bandwidth

**Core Limitations**

- The main quota is that you can have a maximum of 20 VMs per subscription

**Managed data disks**

- CLI, PowerShell

### Create a Linux VM with Azure CLI

Azure Linux VM Demo Summary
This guide summarizes the key steps and takeaways from a demo on creating a Linux VM using Azure CLI, installing an Nginx web server, and verifying its functionality by accessing it via a browser.
Key Takeaways
Prerequisites

Active Azure subscription with appropriate permissions.
Latest Azure CLI installed (Azure CLI Installation).
PowerShell or terminal for running Azure CLI commands.
SSH client for VM access.

Step-by-Step Process

Log in to Azure CLI:

Command: `az login`
Action: Authenticate via browser to connect CLI to your Azure account.
Note: Ensure you're logged in for all CLI operations.

Create a Resource Group:

Command: `az group create --name sbdemo0103 --location eastus`
Purpose: Create a logical container (sbdemo0103) in the eastus region for Azure resources.
Tip: Choose a region close to your location.

![Created Group Resource](image.png)

Create a Linux VM:

Command:

```
az vm create \
 --resource-group sbdemo0103 \
 --name sbdemo0103 \
 --image debian \
 --admin-username azureuser \
 --generate-ssh-keys \
 --public-ip-sku Standard
 --size Standard_B1s
 --location eastus
```

Details:
Creates a Debian-based VM named sbdemo0103 in the sbdemo0103 resource group.
Uses azureuser as the admin username.
Generates SSH keys for secure access.
Assigns a standard public IP address.

Output: Save the public IP address (e.g., 20.124.50.242) for later use.

Install Nginx Web Server:

Command:

```
az vm run-command invoke \
 --resource-group sbdemo0103 \
 --name sbdemo0103 \
 --command-id RunShellScript \
 --scripts "sudo apt-get update && sudo apt-get install -y nginx"
```

Purpose: Runs a shell script on the VM to update packages and install Nginx.

![installed nginx](image-1.png)
Open Port 80:

Command: `az vm open-port --resource-group sbdemo0103 --name sbdemo0103 --port 80`
Purpose: Allows HTTP traffic to the VM to access the Nginx web server.

Verify Web Server:

Action: Open a browser and navigate to the VM’s public IP (e.g., http://20.124.50.242).
Expected Result: Displays the "Welcome to nginx!" page, confirming the VM and web server are operational.

![opened port 80](image-2.png)
![worked](image-3.png)
Clean Up Resources:

Command: `az group delete --name sbdemo0103 --no-wait --yes --verbose`
Purpose: Deletes the resource group and all associated resources (VM, public IP, etc.) to avoid costs.
Note: Deletion may take a few minutes.

Additional Notes

Resource Group: Acts as a logical folder for organizing Azure resources.
SSH Keys: Automatically generated during VM creation for secure Linux VM access.
Public IP: Essential for external access to the VM (e.g., via SSH or browser).
Cost Management: Always delete unused resources to prevent unnecessary charges.

Quick Reference

Login: az login
Resource Group: `az group create --name <name> --location <region>`

Create VM: `az vm create --resource-group <name> --name <vm-name> --image debian --admin-username <username> --generate-ssh-keys --public-ip-sku Standard`

Install Software: `az vm run-command invoke --resource-group <name> --name <vm-name> --command-id RunShellScript --scripts "<linux-commands>"`

Open Port: `az vm open-port --resource-group <name> --name <vm-name> --port 80`

Delete Resources: `az group delete --name <name> --no-wait --yes --verbose`

For detailed documentation, refer to Azure Virtual Machines.

### Create a Linux VM with Azure portal

### Create a Linux VM with Azure PowerShell

### Create a Window VM with Azure CLI

### Create a Window VM with Azure portal

### Create a Window VM with Azure PowerShell

### Connect to a Linux VM

### Connect to a VM running Windows

### Connect to a VM using Secure Shell(SSH)

### Create and manage SSH keys for a Linux VM

### Generate SSH keys in Azure portal

### Generate SSH keys in Azure CLI

### Summarize the key concepts covered in this course

## 1.2 Resource Manager Templates

### Discover the key concepts covered in this course

### Outline the use of Azure Resource Manager (ARM) templates

### Install and set up Visual Studio

### Create JSON templates with Visual Studio

### Create JSON templates with Azure Portal

### Create and deploy a template spec

### Implement deployment scripts to your deployment by using parameters and outputs

### Identify Azure Resource Manger advanced template functionality

### Create and deploy an ARM template

### Deploy a local ARM template

### Use Azure Portal to export a template

### Use the ARM template test toolkit

### Summarize the key concepts covered in this course

## 1.3 Azure Container Registry

### Discover the key concepts covered in this course

### Identify the key features of container registries in Azure

### Create a container registry using Azure portal

### Create a private container registry using Azure CLI

### Create a private container registry using Azure powershell

### Create a geo-replicated container registry using an Azure Resource Manager(ARM) template

### Use the OCI Registry as Storage (ORAS) tool to posh and pull a sample artifact to an Azure container registry

### Create and deploy a container image

### Build and deploy container images using Azure container registry tasks

### Build and store container images with Azure container registry

### Create a Linux container app

### Push an image to your Azure container registry using Docker CLI

### Use the Azure container registry client libraries

### Summarize the key concepts covered in this course

## 1.4 Container Instances

### Discover the key concepts covered in this course

### Provide an overview of Azure Container Instances

### Deploy a container instance using Azure CLI

### Deploy a container instance using Azure Portal

### Deploy a container instance using Azure PowerShell

### Deploy a container instance using Bicep

### Deploy a container instance using Azure Resource Manager(ARM) template

### Deploy a container instance using Docker CLI

### Run a docker container with an Azure container instance

### Deploy a multi-container group using YAML

### Deploy a multi-container group using an ARM template

### Deploy a multi-container group using docker compose

### package a Node.js web application into a container image

### Summarize the key concepts covered in this course

## 1.5 App Service

### Discover the key concepts covered in this course

### List the key features of an Azure App Service

### Identify usage scenarios for app service environments

### Choose an Azure compute service

### Deploy a ASP.NET web app to Azure App Service

### Create a Node.js web app in Azure App Service

### Create a PHP web app in Azure App Service

### Create a Java app on Azure App Service

### Create a Python web app to Azure App Service

### CReate an app service app using an Azure REsource Manager (ARM) template

### Create a custom container in Azure App Service

### Enable diagnostic logging for apps in Azure App Service

### Summarize the key concepts covered in this course

## 1.6 Web Apps

### Discover they key concepts covered in this course

### Run your app in Azure App Service from a ZIP package

### Deploy your code as a ZIP file to Azure App Service

### Sync content from Github to Azure App Service

### Deploy Azure App SErvice using Azure Pipelines

### Deploy microservices predictively in Azure

### Connect to Azure App Service using managed identity

### Add app Authentication to a web app running on Azure App Service

### Configure an Azure App Service app

### Buy a custom domain name for Azure App Service

### Secure a connection to Azure App Service using digital security certificated

### Scale up an app in Azure App Service

### Set up autoscale in Azure App Service

### Summarize the key concepts covered in this course

## 1.7 Azure Functions

### Discover the key concepts covered in this course

### Provide an overview of Azure Functions

### Improve performance add reliability of Azure Functions by implementing best practices

### Provide an overview durable functions

### Create a durable function in C#

### Manage connections in Azure functions

### Connect to events and messaging services from AZure functions

### use security strategies when configuring and running an Azure function

### Outline the use of reliability support in Azure Functions

### monitor function executions using Azure application insights

### Use Azure Functions to deploy code continuously by using source control integration

### Outline the use of deployment slots and how functions are affected by swapping slots

### Manage a function app in Azure Portal

### Identify situations in which you would use custom handlers in Azure Functions

### Summarize the key concepts covered in this course

## 1.8 Azure Functions Triggers

### discover the key concepts covered in this course

### describe key concepts of triggers and bindings

### Configure triggers and bindings in an Azure function

### Run a Azure function from an HTTP request

### Register Azure function biding extensions

### Use binding expressions in an Azure function

### Use binding expressions in an Azure function

### Use the Azure Function return value

### Integrate Azure functions with Blob Storage to build functions that react to changes in blog data

### Work with Azure Cosmos DB bindings in Azure functions

### Handle errors in Azure Function trigger and bindings

### Bring dependencies to Azure Functions

### Summarize the key concepts covered in this course

# 2. Develop for Azure storage

- Develop solutions that use Azure Cosmos DB
- Develop solution that use Azure Blob Storage

## 2.1 Cosmos DB Storage

### Discover the key concepts covered in this course

### Identify the key benefits of Azure Cosmos DB

### Choose the appropriate application programming interface (API) for Azure Cosmos DB storage

### Work with NoSQL databases in the context of Azure Cosmos DBs

### Work with relational databases in the context of Azure Cosmos DBs

### Develop an ASP.NET web application with AZure Cosmos DB for NoSQL

### Use Azure Cosmos DB NoSQL client library for .NET

### Query Azure Cosmos DB by using the API for NoSQL

### Summarize the key concepts covered in this course

## 2.2 Cosmos DB Containers

### Discover the key concepts covered in this course

### Describe indexing in Azure Cosmos DB

### Use SQL queries as a JSON query language

### Describe the Azure Cosmos DB resource model

### Use partitioning and horizontal scaling in Azure Cosmos DB

### Use stored procedures in Azure Cosmos DB

### manage trigger resources using SQL API via REST

### View the change feed in Azure Cosmos DB

### Provision throughput on a container in Azure Cosmos DB for NoSQL

### Set up global distribution on Azure Cosmos DB using API for NoSQL

### View and change the consistency level in Azure Cosmos DB

### Summarize the key concepts covered in this course

## 2.3 Blob Storage

### Discover the key concepts covered in this course

### Describe Blog storage

### Create, upload, download, and list blobs with the Azure Portal

### Create, upload, download, and list blobs with PowerShell

### Create, upload, download, and list blobs with the Azure CLI

### Use Azure Storage Explorer to create a blob

### use .NET to manage properties and metadata fro a blog containers

### Create a project to work with the Azure Blob Storage client library for .NET

### Use Immutable storage with Azure Blob Storage

### Identify the access tiers available for Azure Storage

### Set and change a blob tier

### Rehydrate a blob to an online tier

### Summarize the key concepts covered in this course

# 3. Implement Azure security

- Implement user authentication and authorization
- Implement secure Azure solutions by using Application Insights

## 3.1 Microsoft Identity Platform

## 3.2 Azure Active Directory

## 3.3 Microsoft Graph

# 4. Monitor, troubleshoot, and optimize Azure solutions

- Implement caching for solutions
- Troubleshoot solutions by suing Application Insights

## 4.1 Application Configuration

### Discover the key concepts covered in this course

### Outline Azure App Configuration

### Set up and manage a configuration storeIdentify the structure and organization of key-value pairs

### Organize key-value pairs using labels and hierarchy

### Implement role-based access control (RBAC) to secure access to the configuration store

### use the Azure App Configuration SDK and API to access the configuration store

### Use the Azure App Configuration service to manage feature flags in applications

### Manage Configuration as Code using Azure DevOps

### Set up automatic refresh policies to keep applications in sync with the configuration store

### Store and manage secrets in the Azure App Configuration service

### Monitor the usage and performance of the configuration store

### Set up a hig availability configuration store across multiple regions

### Summarize the key concepts covered in this course

## 4.2 Caching in Azure

### Discover the key concepts covered in this course

### Outline how caching is used

### Provide and overview of Azure Front Door

### Cache with Azure Front Door

### Provide an overview of Azure Content Delivery Network (CDN)

### Control Azure CDN caching behavior

### Create an Azure CDN endpoint

### Provide and overview of Azure Cache for Redis

### Create an open-source Redis cache

### Summarize the key concepts covered in this course

## 4.3 Application Troubleshooting

### Discover the key concepts covered in this course

### Provide an overview of Azure Monitor and its capabilities for monitoring and logging

### Use Log Analytics to collect, analyze, and troubleshoot log data

### Enable Azure Diagnostics to gather data for troubleshooting and analysis

### Use Event Hubs to collect and stream log data for real-time analysis

### Utilize application insights to monitor the performance and usage of web applications

### Identify the different types of logs available in Azure

### Use Azure Metrics to analyze the performance and usage of Azure resources and applications

### Configure Azure Alerts to notify you of specific conditions or changes in Azure resources and applications

### Create and use dashboards to visualize log and metric data

### Automate troubleshooting tasks using Azure Automation run-books and PowerShell

### Use Azure Network Watcher to troubleshoot and diagnose network issues

### Use Microsoft Defender for Cloud to detect and diagnose security=related issues in Azure resources and applications

### Summarize the key concepts covered in this course

# 5. Connect to and consume Azure services and third-party services

- Implement API Management

## 5.1 Azure API Management

### Discover the key concepts covered in this course

### Describe Azure API Management

### Set up and configure API gateways to control access to backend services and APIs

### Implement security measures such as authentication and authorization to protect APIs

### Use Azure API Management policies sto transform and manipulate incoming and outgoing requests and responses

### Implement and manage different versions of an API

### Set up and customize a developer portal to provide documentation and access to APIs

### Configure caching to improve the performance of APIs

### Implement rate limits and quotas to control usage and prevent overuse of resources

### Set up and use monitoring and analytics to understand the usage and performance of APIs

### Integrate AZure API Management with Azure Functions to create serverless APIs

### Integrate Azure API management with Azure Active Directory(AD) to secure APIs using Azure AD

### Identify options for deploying and scaling Azure API Management to meet your needs

### Summarize the key concepts covered in this course

## 5.2 Azure Event Grid

### Discover they key concepts covered in this course

### Describe the concepts of Azure Event Grid

### Identify the different types of events that can be handled by Azure Event Grid

### Create and manage event subscriptions in Azure Event Grid

### Create custom topics and event types in Azure Event Grid

### Filter events based on specific criteria, such as event type or data

### Route events to different handlers such as Azure Functions or Logic Apps

### Configure dead lettering for event subscriptions

### Authenticate and authorize access to events and event handlers

### Replay events and configure event replay for event subscriptions

### Use Azure Event Grid and Azure Functions to create and manage event-triggered Azure Functions

### Use Event Grid with Logic Apps to create and manage event-triggered Logic Apps

### Use Event Grid with Events Hub to create and manage event-triggered Event Hubs

### Summarize the concepts covered in this course

## 5.3 Message-based solutions

### Discover the key concepts covered in this course

### Provide an overview of Azure Service Bus

### Create, manage, and use queues to send and receive messages.

### Implement Azure Service Bus and Queue Storage to build more powerful messaging solutions

### Identify the different messaging patterns available in Azure Service Bus

### Outline Service Bus Security

### Use Service Bus conjunction with Event Grid to build event-driven architectures

### Use Service Bus and Azure Functions together to build serverless messaging solutions

### Use Service Bus and Logic Apps together to build workflows and automate business processes

### Use Service Bus and Power Automate together to automate business processes

### Use Service Bus in conjunction with Azure Stream Analytics to build data processing solutions

### Outline Queue Storage scalability and performance

### Outline the use of Service Bus and Queue Storage monitoring

### Summarize the key concepts covered in this course.
