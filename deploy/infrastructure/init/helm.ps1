# Use Chocolatey on Windows
# 注：安装的时候需要保证网络能够访问googleapis这个域名
# 本行命令需要需要管理员身份
choco install kubernetes-helm

# Change helm repo
helm repo add stable http://mirror.azure.cn/kubernetes/charts/

# Update charts repo
helm repo update