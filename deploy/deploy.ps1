kubectl create configmap geektime-ordering-api-config --from-file=geektime-ordering-api/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\geektime-ordering-api\geektime-ordering-api.yaml

kubectl create configmap geektime-mobile-apiaggregator-config --from-file=geektime-mobile-apiaggregator/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\geektime-mobile-apiaggregator\geektime-mobile-apiaggregator.yaml



kubectl create configmap geektime-config --from-env-file=env.txt -o yaml --dry-run | kubectl apply -f - 

kubectl create configmap geektime-mobile-gateway-config --from-file=geektime-mobile-gateway/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\geektime-mobile-gateway\geektime-mobile-gateway.yaml
kubectl apply -f .\geektime-mobile-gateway\geektime-mobile-gateway-ingress.yaml


kubectl create configmap geektime-healthcheckshost-config --from-file=geektime-healthcheckshost/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\geektime-healthcheckshost\geektime-healthcheckshost.yaml

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit