helm repo add stable http://mirror.azure.cn/kubernetes/charts/
helm repo update
helm upgrade prometheus-operator stable/prometheus-operator --version 8.10.0 --values .\prometheus-operator\values.yaml  --namespace kube-system