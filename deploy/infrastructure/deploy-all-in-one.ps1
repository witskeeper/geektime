kubectl create configmap fluentd-config --from-file=fluentd -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .