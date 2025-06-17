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

#### Prerequisites

- Latest version of Azure PowerShell installed
- Azure account with appropriate permissions

#### Step-by-Step Process

##### 1. Connect to Azure

```powershell
Connect-AzAccount
```

- Opens browser for authentication
- Sign in with your Azure credentials
- Close browser and return to PowerShell

##### 2. Create Resource Group

```powershell
New-AzResourceGroup -Name sbdemo0105 -Location EastUS
```

**Key Points:**

- Resource group = logical container for resources
- Use capitalization for all Azure PowerShell commands
- Choose location close to your region

##### 3. Create Linux VM

```powershell
New-AzVm -ResourceGroupName sbdemo0105 -Name sbdemo0105 -Location EastUS -Image Debian -Size Standard_B2s -PublicIPAddressName sbdemo0105 -OpenPorts 80 -GenerateSshKey -SshKeyName sbdemo0105
```

**Command Breakdown:**

- `-ResourceGroupName`: Specify the resource group
- `-Name`: VM name
- `-Location`: Azure region
- `-Image`: OS image (Debian in this case)
- `-Size`: VM size (Standard_B2s)
- `-PublicIPAddressName`: Public IP resource name
- `-OpenPorts 80`: Open port 80 for web traffic
- `-GenerateSshKey`: Generate SSH keys automatically
- `-SshKeyName`: Name for the SSH key

**During Creation:**

- Prompts for username and password
- Generates private and public SSH keys
- Process takes several minutes

##### 4. Install Web Server on VM

```powershell
# Create bash script
echo 'sudo apt-get update && sudo apt-get install -y nginx' > sbdemo0105.sh

# Execute script on VM
Invoke-AzVMRunCommand -ResourceGroup sbdemo0105 -Name sbdemo0105 -CommandId RunShellScript -ScriptPath \sbdemo0105.sh
```

**Key Points:**

- Can't execute commands directly on VM
- Must create a script file first
- Script installs Nginx web server

##### 5. Get VM Public IP Address

```powershell
Get-AzPublicIPAddress -ResourceGroupName sbdemo0105 -Name sbdemo0105 | select "IPAddress"
```

- Retrieves the public IP address of the VM
- Use this IP to access the web server

##### 6. Test the Web Server

- Copy the IP address from PowerShell output
- Navigate to the IP address in your browser
- Should see "Welcome to nginx!" page

##### 7. Clean Up Resources

```powershell
Remove-AzResourceGroup -Name sbdemo0105
```

- Deletes the entire resource group and all resources within it
- Confirm deletion when prompted

#### Important Notes

##### Azure PowerShell Syntax Rules

- All Azure PowerShell commands use capitalization
- Resource groups are logical containers for resources
- Most everything in Azure is a resource

##### VM Creation Process

- VM creation takes several minutes
- SSH keys are automatically generated
- Public IP is created as a separate resource

##### Security Considerations

- Port 80 opened for web traffic
- SSH keys generated for secure access
- Username and password required during creation

##### Best Practices

- Always clean up resources after testing to avoid charges
- Choose appropriate VM sizes for your needs
- Select regions close to your location for better performance

#### Quick Reference Commands

```powershell
# Connect to Azure
Connect-AzAccount

# Create resource group
New-AzResourceGroup -Name <name> -Location <location>

# Create VM
New-AzVm -ResourceGroupName <rg-name> -Name <vm-name> -Location <location> -Image <image> -Size <size> -PublicIPAddressName <ip-name> -OpenPorts <port> -GenerateSshKey -SshKeyName <key-name>

# Run command on VM
Invoke-AzVMRunCommand -ResourceGroup <rg-name> -Name <vm-name> -CommandId RunShellScript -ScriptPath <script-path>

# Get public IP
Get-AzPublicIPAddress -ResourceGroupName <rg-name> -Name <ip-name> | select "IPAddress"

# Clean up
Remove-AzResourceGroup -Name <rg-name>
```

### Create a Window VM with Azure CLI

#### Prerequisites

- Azure CLI installed and configured
- Azure account with appropriate permissions
- PowerShell or command prompt access

#### Step-by-Step Process

##### 1. Create Resource Group

```bash
az group create -n sbdemo0106 --location eastus
```

**Key Points:**

- Resource group = logical container for resources
- `-n`: Name parameter (short form)
- `--location`: Azure region for deployment
- Choose location close to your region for better performance

##### 2. Create Windows VM

```bash
az vm create -g sbdemo0106 -n sbdemo0106 --image Win2019Datacenter --admin-username azureuser --public-ip-sku Standard --size Standard_B1s
```

**Command Breakdown:**

- `-g`: Resource group name (short form of `--resource-group`)
- `-n`: VM name (short form of `--name`)
- `--image`: OS image (Win2019Datacenter)
- `--admin-username`: Administrator account name
- `--public-ip-sku`: Public IP type (Standard)
- `--size Standard_B1s`: Size of VM

**During Creation:**

- Prompts for administrator password: SuperPa$$w0rd
- Confirm password
- Process takes several minutes
- **Important**: Save the public IP address from output

![alt text](image-4.png)

##### 3. Install Web Server on VM

```bash
az vm run-command invoke -g sbdemo0106 -n sbdemo0106 --command-id RunPowerShellScript --scripts "Install-WindowsFeature -name Web-Server -IncludeManagementTools"
```

**Key Points:**

- Uses `vm run-command invoke` to execute commands remotely
- `--command-id RunPowerShellScript`: Specifies PowerShell execution
- `Install-WindowsFeature`: Windows PowerShell command to install IIS
- `-name Web-Server`: Installs Internet Information Services (IIS)
- `-IncludeManagementTools`: Includes management tools for IIS

![alt text](image-5.png)

##### 4. Open Port 80 for Web Traffic

```bash
az vm open-port -g sbdemo0106 -n sbdemo0106 --port 80
```

**Important:**

- Port 80 must be opened for HTTP web traffic
- This modifies the Network Security Group (NSG) rules
- Without this step, web server won't be accessible from internet

![alt text](image-6.png)

##### 5. Test the Web Server

- Use the public IP address from step 2 output
- Navigate to the IP address in your browser
- Should see "Internet Information Services" default page
- Confirms VM is working and web server is accessible

##### 6. Clean Up Resources

```bash
az group delete -n sbdemo0106
```

- Deletes the entire resource group and all contained resources
- Confirm deletion when prompted (type 'y')
- Prevents ongoing charges for unused resources

#### Important Notes

##### Azure CLI Syntax

- Uses lowercase commands with hyphens
- Short forms available: `-g` for `--resource-group`, `-n` for `--name`
- Parameter values are case-sensitive (e.g., `Win2019Datacenter`, `Standard`)

##### Windows VM Specifics

- Default image: Windows Server 2019 Datacenter
- Uses PowerShell for remote command execution
- IIS (Internet Information Services) as the web server
- Requires password authentication for admin user

##### Network Configuration

- Public IP created automatically
- Port 80 must be explicitly opened for web access
- Network Security Group rules are modified automatically

##### Security Considerations

- Administrator username and password required
- Public IP exposes VM to internet
- Only necessary ports should be opened
- Always clean up test resources

#### Quick Reference Commands

```bash
# Create resource group
az group create -n <resource-group-name> --location <location>

# Create Windows VM
az vm create -g <resource-group> -n <vm-name> --image Win2019Datacenter --admin-username <username> --public-ip-sku Standard

# Install web server
az vm run-command invoke -g <resource-group> -n <vm-name> --command-id RunPowerShellScript --scripts "Install-WindowsFeature -name Web-Server -IncludeManagementTools"

# Open port 80
az vm open-port -g <resource-group> -n <vm-name> --port 80

# Delete resource group
az group delete -n <resource-group-name>
```

#### Comparison with Linux VM Creation

| Aspect          | Windows VM                   | Linux VM                 |
| --------------- | ---------------------------- | ------------------------ |
| Image           | `Win2019Datacenter`          | `Ubuntu2204` or `Debian` |
| Authentication  | Username/Password            | SSH Keys (preferred)     |
| Web Server      | IIS (Install-WindowsFeature) | Nginx/Apache (apt-get)   |
| Remote Commands | RunPowerShellScript          | RunShellScript           |
| Default Ports   | 3389 (RDP), 80 (HTTP)        | 22 (SSH), 80 (HTTP)      |

#### Best Practices

- Always note the public IP address during VM creation
- Test connectivity before installing software
- Open only necessary ports for security
- Use strong passwords for Windows VMs
- Clean up resources immediately after testing
- Consider using parameter files for complex deployments

### Create a Window VM with Azure portal

### Create a Window VM with Azure PowerShell

### Connect to a Linux VM

### Connect to a VM running Windows

### Connect to a VM using Secure Shell(SSH)

### Create and manage SSH keys for a Linux VM

### Generate SSH keys in Azure portal

### Generate SSH keys in Azure CLI

#### Prerequisites

- Azure CLI installed and configured
- Existing Azure VM (Linux-based)
- PowerShell or command prompt access
- Resource group with VM already created

#### Step-by-Step Process

##### 1. Create SSH Key Pair in Azure

```bash
az sshkey create -g sbdemo0110 -n sbdemo0114
```

**Command Breakdown:**

- `az sshkey create`: Creates SSH key pair in Azure
- `-g`: Resource group name (short form)
- `-n`: Name for the SSH key resource

**What Happens:**

- Creates both private and public key
- Saves keys locally in specified folder (usually `.ssh` directory)
- Stores public key in Azure for VM access
- Returns file paths for both keys

##### 2. Get VM Public IP Address

```bash
az network public-ip show -g sbdemo0110 -n sbdemo0110-ip | ConvertFrom-Json | select "ipAddress"
```

**Command Breakdown:**

- `az network public-ip show`: Retrieves public IP information
- `-g`: Resource group name
- `-n`: Public IP resource name (usually `<vm-name>-ip`)
- `| ConvertFrom-Json`: Converts JSON output to PowerShell object
- `| select "ipAddress"`: Extracts only the IP address value

**Important Notes:**

- Public IP resource typically named `<VM-name>-ip`
- JSON conversion needed for PowerShell processing
- Case-sensitive: `"ipAddress"` (lowercase 'i', uppercase 'A')

##### 3. Connect to VM Using SSH Key

```bash
ssh -i .\.ssh\<key-filename>.pub azureuser@<IP-ADDRESS>
```

**Example:**

```bash
ssh -i .\.ssh\1677784226_5817175.pub azureuser@4.154.67.226
```

**Key Points:**

- `-i`: Specifies identity file (private key)
- Path to private key file in `.ssh` directory
- `azureuser`: Default username (or your specified username)
- Use the IP address obtained from step 2

##### 4. Upload Existing SSH Key to Azure

```bash
az sshkey create -g sbdemo0110 -n sbdemo0114-2 --public-key "@.\.ssh\<key-filename>.pub"
```

**Command Breakdown:**

- Same `az sshkey create` command
- `--public-key`: Specifies existing public key file
- `@` symbol: Critical - tells Azure this is a file reference, not the key content
- Must use public key file (.pub extension)

**Important:**

- The `@` symbol is crucial - without it, Azure interprets the path as the actual key content
- Only uploads public key to Azure
- No private key is generated or returned
- Existing private key remains on local system

#### Key Concepts

##### SSH Key Management in Azure

- **Public Key**: Stored in Azure and on local system
- **Private Key**: Only stored on local system (never shared)
- **Key Names**: Can be different from VM names
- **Resource Groups**: Best practice to store keys in same RG as VM

##### File Permissions (Linux Systems)

- Microsoft recommends changing private key permissions on Linux
- Use `chmod` utility to secure private key file
- Not required for Windows systems in this demo

##### Public IP Naming Convention

- Default pattern: `<VM-name>-ip`
- Can be customized during VM creation
- Must match exact name when querying

#### Security Best Practices

##### Key Management

- Delete resource groups after demos to remove keys
- Keep private keys secure and never share
- Use proper file permissions on Linux systems
- Regularly rotate SSH keys for production systems

##### Connection Security

- Always use SSH keys instead of passwords when possible
- Verify IP addresses before connecting
- Use `exit` command to properly close SSH sessions
- Monitor SSH access logs

#### Quick Reference Commands

```bash
# Create new SSH key in Azure
az sshkey create -g <resource-group> -n <key-name>

# Get VM public IP
az network public-ip show -g <resource-group> -n <public-ip-name> | ConvertFrom-Json | select "ipAddress"

# Connect via SSH
ssh -i <path-to-private-key> <username>@<ip-address>

# Upload existing public key
az sshkey create -g <resource-group> -n <key-name> --public-key "@<path-to-public-key>"

# Exit SSH session
exit
```

#### Troubleshooting Tips

##### Common Issues

- **Permission Denied**: Check private key file permissions
- **Key Not Found**: Verify file path and extension (.pub for public key)
- **Connection Timeout**: Verify IP address and network security group rules
- **Authentication Failed**: Ensure using correct private key file

##### File Path Issues

- Windows: Use `.\` for current directory
- Backslashes in Windows paths: `\.ssh\`
- Forward slashes in Linux paths: `/.ssh/`

##### JSON Conversion Problems

- Ensure `ConvertFrom-Json` is properly capitalized
- Check for proper pipe symbols `|`
- Verify JSON output format from Azure CLI

#### Comparison: Create vs Upload

| Method              | Command                                                     | Use Case           | Output                       |
| ------------------- | ----------------------------------------------------------- | ------------------ | ---------------------------- |
| **Create New**      | `az sshkey create -g <rg> -n <name>`                        | Need new key pair  | Both private and public keys |
| **Upload Existing** | `az sshkey create -g <rg> -n <name> --public-key "@<file>"` | Have existing keys | Confirmation only            |

#### Best Practices Summary

- Store SSH keys in same resource group as VM
- Use descriptive names for key resources
- Always use `@` symbol when referencing key files
- Clean up demo resources to remove keys
- Test connectivity after key creation
- Keep private keys secure and local-only

##### Summarize the key concepts covered in this course

## 1.2 Resource Manager Templates

### Discover the key concepts covered in this course

#### Course Introduction

Azure Resource Manager (ARM) templates are essential tools for managing Azure resources in a consistent and automated manner. This course provides comprehensive training on creating and deploying ARM templates using various tools and methodologies.

#### Learning Objectives

##### Core Concepts

- Understanding ARM template fundamentals
- Working with JSON template structure
- Template specification requirements

##### Tools and Techniques

- **Visual Studio Code** - Primary development environment
- **Azure Portal** - Web-based template management
- **Deployment Scripts** - Automation of template deployment processes

##### Key Capabilities Covered

- Creating and deploying templates locally
- Exporting existing templates for reuse
- Template quality assurance and testing
- Automated deployment workflows

#### Course Structure

The course follows a hands-on approach with:

- **Practical exercises** throughout each module
- **Lab assignments** for real-world application
- **Progressive complexity** from basics to advanced concepts

---

#### Summary

This course serves as a comprehensive introduction to ARM templates, covering everything from basic JSON template creation to advanced deployment automation. Students will gain practical experience with industry-standard tools while preparing for the AZ-204 certification exam.

#### Key Points

- ARM templates enable consistent, automated Azure resource management
- Multiple tools available: Visual Studio Code and Azure Portal
- Course includes both theoretical knowledge and hands-on practice
- Directly aligned with AZ-204 certification requirements
- Emphasis on automation and quality assurance practices

#### Important Concepts

- **ARM Templates**: Infrastructure as Code (IaC) for Azure
- **JSON Structure**: Template format and syntax
- **Deployment Scripts**: Automation capabilities
- **Template Export**: Reusability and sharing
- **Local Development**: Offline template creation and testing

#### Questions/Notes

- What specific Azure resources will be covered in the hands-on labs?
- Are there prerequisites for Visual Studio Code setup?
- How does this course integrate with other AZ-204 preparation materials?

### Outline the use of Azure Resource Manager (ARM) templates

#### What are ARM Templates?

ARM templates are **JSON-formatted files** that define the infrastructure and configuration of Azure projects using **declarative syntax**. They serve as Infrastructure as Code (IaC) to specify all Azure resources you want to deploy along with their properties and configurations.

##### Alternative: Bicep

- ARM templates can also be written using **Bicep** (a coding language)
- Bicep templates ultimately compile into JSON format

#### Key Benefits of ARM Templates

##### Repeatability

- **Consistent results** every time the template is executed
- Same outcome regardless of how many times you run the template

##### Orchestration

- Deploy **multiple resources** in a single template
- Define **dependencies** between resources
- Control **deployment order** automatically

##### Extensibility

- Use **deployment scripts** to execute custom PowerShell or Bash scripts
- Enables virtually unlimited customization during deployment

##### Validation & Testing

- **Pre-deployment validation** ensures templates work before actual deployment
- **What-if operations** preview changes without deploying
- Built-in Azure validation checks template correctness

##### Modularity

- Create **modular templates** instead of monolithic files
- Main template can reference **separate module files**
- Modules can be stored in **central repositories**

#### ARM Template Structure

ARM templates contain specific sections:

##### Core Sections

- **Parameters**: Input values required from users when running the template
- **Variables**: Generated values within the template for easier coding
- **Resources**: Main section containing definitions of Azure resources to deploy
- **Outputs**: Values returned after template execution
- **User-defined Functions**: Reusable functions for common operations (e.g., key acquisition)

#### Deployment Methods

##### Available Tools

- **Azure Portal**: Browser-based custom template import and deployment
- **Azure Cloud Shell**: Command-line access within the portal
- **Azure CLI**: Local system command execution (recommended by instructor)
- **Azure PowerShell**: PowerShell-based deployment commands
- **Azure REST API**: Advanced option for direct API calls
- **GitHub Integration**: Deploy-to-Azure buttons in repositories

#### Template Limitations

##### Size and Quantity Constraints

- Maximum template size: **4 megabytes**
- Maximum parameters: **256**
- Maximum variables: **256**
- Maximum resources: **800**
- Maximum unique locations: **10** (best practice: deploy to same region)

#### Development Tools

##### Visual Studio Code (Primary Tool)

- **ARM template language support**
- **Bicep plugin** integration
- **Community resource snippets**
- **Auto-completion** features
- Enhanced productivity for template creation

##### ARM Template Test Toolkit

- **Microsoft-provided script** for validation
- Checks **best practices compliance**
- **Error detection** before deployment
- **Suggestion engine** for improvements and updates

#### Use Cases

##### Enterprise Applications

- **Policy as Code**: Centralized policy management through version control
- **CI/CD Integration**: Seamless deployment pipeline integration
- **Blueprint Templates**: Pre-built templates for common scenarios
- **Resource Export**: Generate templates from existing deployments

---

#### Summary

ARM templates are the **de facto standard** for Azure resource management, providing a declarative approach to infrastructure deployment. They enable consistent, repeatable, and automated resource provisioning while supporting complex orchestration and validation workflows.

#### Key Points

- ARM templates use JSON declarative syntax for infrastructure definition
- Provide repeatable, consistent deployment results across environments
- Support complex orchestration with resource dependencies
- Include built-in validation and testing capabilities
- Integrate seamlessly with modern DevOps practices and CI/CD pipelines
- Visual Studio Code is the recommended development environment

#### Important Concepts

- **Declarative Syntax**: Describe desired state rather than deployment steps
- **Infrastructure as Code**: Version-controlled, testable infrastructure definitions
- **Template Modularity**: Reusable components and centralized repositories
- **What-if Operations**: Safe preview of deployment changes
- **Resource Orchestration**: Automated dependency management and deployment ordering

#### Questions/Notes

- How do ARM templates compare to other IaC tools like Terraform?
- What are the performance implications of template size limits?
- Best practices for organizing complex multi-resource deployments?
- Integration patterns with existing CI/CD pipelines?

### Install and set up Visual Studio

#### 1. ✅ Download & Install VS Code

- Open browser → Search: `Download Visual Studio Code`
- Go to: [https://code.visualstudio.com/download](https://code.visualstudio.com/download)
- Download the **User Installer x64** (for Windows)
- Follow Setup Wizard:
  - Accept license agreement
  - Use default install location
  - Add **"Open with Code"** to Windows Explorer context menu
  - Click **Install** and then **Finish** to launch VS Code

---

#### 2. 🧭 Explore Visual Studio Code Interface

- **Left Toolbar Icons:**
  - 📁 File Explorer
  - 🔍 Search
  - 🔄 Source Control (e.g., Git)
  - 🐞 Debug
  - 🧩 Extensions
- **Bottom Left:** Profile & Settings
- **Top Menu:** File, Edit, View, Run, etc.

---

#### 3. 💡 ARM Templates & Bicep

- ARM templates are **JSON files** used to define Azure infrastructure
- **Bicep** is a higher-level syntax that compiles into ARM JSON
  - More readable and maintainable
  - Better suited for developers

---

#### 4. ⚙️ Install Bicep Extension

- Click **Extensions** icon (last on left toolbar)
- Search for **Bicep**
- Install the main Bicep extension

---

#### 5. 📂 Create a Project Folder

- Go to: **File → Open Folder**
- Create folders:
  - `C:\work\cldsaz2302`
- Select the folder and click **Select Folder**
- Accept the **"Do you trust this folder?"** prompt

---

#### ✅ Final Setup Summary

- Installed **VS Code**
- Added **Bicep extension**
- Created a **project folder**
- Ready to begin writing and managing **ARM templates**

> You are now fully set up to start your journey with Infrastructure as Code (IaC) on Azure using VS Code and Bicep.

### Create JSON templates with Visual Studio

a demonstration is provided on how to create an Azure Resource Manager (ARM) template using Visual Studio Code (VS Code) with the Bicep language, which is then compiled into a JSON template. The demo focuses on creating a simple ARM template to deploy an Azure storage account, covering key components like variables, parameters, resources, tags, and outputs. It highlights the use of Bicep for easier template creation and the process of compiling it to JSON for comparison. This is part of a course preparing learners for the AZ-204: Developing Solutions for Microsoft Azure certification exam.

#### Key Steps and Concepts:

- **Setup**:

  - Open VS Code and access a project folder (e.g., `CLDSAZ2302`) created previously with the Bicep plug-in installed.
  - Create a new Bicep file named `azuredeploy.bicep` in the project folder via the Explorer panel.

- **Creating the ARM Template in Bicep**:

  - **Variable**: Define a variable `storageAccountName` with the value `'sbdemo0204'`. VS Code flags unused variables (underlined) to ensure proper usage.
  - **Resource Definition**:
    - Define a storage account resource named `storageAccount` using the `Microsoft.Storage/storageAccounts` resource type (version 2022-09-01).
    - Set properties:
      - `name`: Reference the `storageAccountName` variable.
      - `location`: Initially hardcoded as `'EastUS'`, later replaced with a parameter.
      - `sku`: Set to `Standard_LRS` (Locally Redundant Storage).
      - `kind`: Set to `StorageV2`.
  - **Parameter**:
    - Add a `location` parameter with a default value tied to the resource group’s location (`resourceGroup().location`) to avoid hardcoding, resolving the underlined location error.
    - Replace the hardcoded `'EastUS'` with the `location` parameter.
  - **Tags**:
    - Add tags for better resource management and billing queries:
      - `environment: 'dev'`
      - `business_unit: 'connectivity'`
  - **Output**:
    - Define an output `accountName` to return the `storageAccountName` value after deployment.
  - Save the file (`Ctrl+S`).

- **Compiling Bicep to JSON**:

  - Compile the Bicep file to JSON using `Ctrl+Shift+B` or by right-clicking the file and selecting "Build ARM Template."
  - This generates a new file, `azuredeploy.json`, in the Explorer panel.
  - The JSON file includes:
    - Metadata about the file.
    - Parameters section with the `location` parameter.
    - Variables section with `storageAccountName`.
    - Resources section defining the storage account.
    - Outputs section with the `accountName` output.

- **Key Features Highlighted**:

  - **Bicep Advantages**: Bicep simplifies ARM template creation with a more readable syntax and features like autocomplete in VS Code, making complex templates easier to manage.
  - **VS Code Support**: The Bicep plug-in provides autocomplete, error detection (e.g., unused variables, hardcoded locations), and compilation to JSON.
  - **JSON Comparison**: The compiled JSON closely mirrors the Bicep file but is more verbose, reinforcing Bicep’s ease of use for complex templates.

- **Code Example** (Bicep):
  ```bicep
  param location string = resourceGroup().location
  var storageAccountName = 'sbdemo0204'
  resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
    name: storageAccountName
    location: location
    sku: {
      name: 'Standard_LRS'
    }
    kind: 'StorageV2'
    tags: {
      environment: 'dev'
      business_unit: 'connectivity'
    }
  }
  output accountName string = storageAccountName
  ```

#### Course Context:

- The demo emphasizes practical ARM template creation using VS Code and Bicep, with hands-on steps to build, validate, and compile templates.
- It prepares learners for the AZ-204 exam by demonstrating real-world Azure resource management techniques.

### Create JSON templates with Azure Portal

**Objective:**  
Use the **Azure Portal** to generate, review, and optionally deploy an **ARM (JSON) template** using built-in quickstart templates.

---

#### 🧰 Tools Used

- Azure Portal (https://portal.azure.com)

---

#### 🪜 Step-by-Step Instructions

##### 1. Access the Custom Template Feature

- Go to Azure Portal
- In the **search bar**, type:  
  `deploy a custom template`
- Click **"Deploy a custom template"** from the results

---

##### 2. Load a Quickstart Template

- On the "Custom deployment" screen:
  - Choose **"Quickstart template"** as the source
  - In the dropdown, search for:  
    `storage-account-create`
  - Select: `microsoft.storage/storage-account-create`
- Click **Edit template**

---

##### 3. Review the Auto-Generated JSON Template

- The editor displays a **JSON ARM template**
- Key elements in the template:

```jsonc
{
  // Metadata at the top
  "parameters": {
    "location": { ... },
    "storageAccountName": { ... },
    ...
  },
  "resources": [
    {
      "type": "Microsoft.Storage/storageAccounts",
      "apiVersion": "2022-09-01",
      "name": "[parameters('storageAccountName')]",
      "location": "[parameters('location')]",
      "sku": { "name": "Standard_LRS" },
      "kind": "StorageV2"
    }
  ],
  "outputs": {
    "storageAccountName": { ... },
    "storageAccountId": { ... }
  }
}
```

### Create and deploy a template spec

#### 1. Create a Template Spec via Azure CLI

##### az login

```
az login

```

##### Create a resource group for Spec

```
az group create -n specrg --location eastus
```

##### Create the template the spec

```
az ts create \
  -g specrg \
  -n storageSpec \
  --version "1.0" \
  --location eastus \
  --template-file .\azuredeploy.json

```

### 3. Deploy resources from the template Spec

#### Create a Deployment Resource Group

```
az group create -n deployrg --location eastus

```

#### Get the Template Spec Id

```
$id = az ts show -g specrg -n storageSpec --version "1.0" --query "id"

echo $id

```

#### Deploy the Template Spec

```
az deployment group create \
  -g deployrg \
  --template-spec $id \
  --parameters storageAccountName='sbdemo0206'

```

#### Final verification

- Open Azure Portal → `Resource Groups`

- Open `deployrg`

- Confirm presence of `sbdemo0206` storage account

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

This demonstration shows how to create an Azure Container Registry using the Azure CLI as an alternative to the Azure Portal approach covered in the previous demo.

### Prerequisites

- Azure CLI installed and running latest version
- PowerShell or command line interface access
- Azure subscription with appropriate permissions

### Step-by-Step Process

#### 1. Azure Authentication

First, authenticate with Azure using the login command:

```bash
az login
```

**Process:**

- Command opens web browser for authentication
- Select your Azure account
- Browser confirms successful login
- Return to command line interface

#### 2. Create Resource Group

Before creating the Container Registry, a resource group is required:

```bash
az group create -n sbdemo0304 --location eastus
```

**Command breakdown:**

- `az group create`: Creates new resource group
- `-n sbdemo0304`: Names the resource group
- `--location eastus`: Specifies deployment region

#### 3. Create Container Registry

Create the Azure Container Registry within the resource group:

```bash
az acr create -g sbdemo0304 -n sbdemo0304 --sku Basic
```

**Command breakdown:**

- `az acr create`: Creates new Azure Container Registry
- `-g sbdemo0304`: Specifies resource group name
- `-n sbdemo0304`: Names the container registry
- `--sku Basic`: Sets pricing tier (Basic for cost efficiency)

#### 4. Registry Login

Authenticate to the newly created registry for image operations:

```bash
az acr login -n sbdemo0304
```

**Purpose:**

- Enables pushing and pulling container images
- Required before any registry operations
- Uses registry name for authentication

### Key Registry Information

#### Login Server Property

The command output includes a `loginServer` property which provides:

- Registry's login server name
- Essential for registry interactions
- Alternative to using registry name depending on operation type

### Cleanup Process

Remove all created resources to avoid ongoing charges:

```bash
az group delete -n sbdemo0304
```

**Effect:**

- Deletes the entire resource group
- Automatically removes the Container Registry
- Confirms deletion before execution

---

#### Summary

This demo successfully demonstrates creating an Azure Container Registry using just **two primary commands**: resource group creation and registry creation. The Azure CLI provides a streamlined, scriptable approach to registry deployment compared to the portal interface.

#### Key Points

- Azure CLI offers efficient command-line alternative to portal-based registry creation
- Resource group creation is prerequisite for Container Registry deployment
- Basic SKU provides cost-effective option for development and testing scenarios
- Registry login is required before performing push/pull operations
- Cleanup through resource group deletion removes all associated resources

#### Important Commands

- **Authentication**: `az login` - Connect to Azure account
- **Resource Group**: `az group create` - Create container for resources
- **Registry Creation**: `az acr create` - Deploy Container Registry
- **Registry Access**: `az acr login` - Authenticate to registry
- **Cleanup**: `az group delete` - Remove all resources

#### Questions/Notes

- What are the differences between Basic, Standard, and Premium SKUs?
- How does CLI authentication compare to service principal authentication?
- Can registry creation be automated through scripts or CI/CD pipelines?
- What are the networking considerations for private registries?

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

```

```
