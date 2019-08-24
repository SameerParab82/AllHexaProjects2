Connect-AzAccount #(PowerShell Core)
Login-AzAccount  #(Same as Connect-AzAccount)
Login-AzureRmAccount #(PowerShell)

#verb-noun
$PSVersionTable.PSVersion
Install-Module -Name Az -AllowClobber -Scope CurrentUser

GET-AzureRmResourceGroup


New -AzureRmResourceGroupDeployment -Name "MyDeploy"
     -ResourceGroupName "MyDeployGroup" 
     -TemplateFile "Path of azuredeploy.json"
     -TemplateParameterFile "Path of azuredeploy.parameter.json"