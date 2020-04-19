kubectl create configmap geektime-ordering-api-config --from-file=geektime-ordering-api/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap geektime-identity-api-config --from-file=geektime-identity-api/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap geektime-mobile-apiaggregator-config --from-file=geektime-mobile-apiaggregator/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap geektime-config --from-env-file=env.txt -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap geektime-mobile-gateway-config --from-file=geektime-mobile-gateway/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap geektime-healthcheckshost-config --from-file=geektime-healthcheckshost/configs -o yaml --dry-run | kubectl apply -f - 

helm install geektime-ordering-api .\charts\geektime-ordering-api -n default
helm install geektime-identity-api .\charts\geektime-identity-api -n default
helm install geektime-mobile-apiaggregator .\charts\geektime-mobile-apiaggregator -n default
helm install geektime-mobile-gateway .\charts\geektime-mobile-gateway -n default
helm install geektime-healthcheckshost  .\charts\geektime-healthcheckshost -n default

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit