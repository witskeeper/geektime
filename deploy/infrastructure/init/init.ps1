kubectl apply -f mandatory.yaml
kubectl apply -f ingress-port.yaml
kubectl apply -f kubernetes-dashboard.yaml
kubectl apply -f kubernetes-dashboard-ingress.yaml

$TOKEN=((kubectl -n kube-system describe secret default | Select-String "token:") -split " +")[1]
kubectl config set-credentials docker-for-desktop --token="${TOKEN}"
echo $TOKEN

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit