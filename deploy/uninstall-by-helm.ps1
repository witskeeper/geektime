helm uninstall geektime-ordering-api  -n default
helm uninstall geektime-identity-api  -n default
helm uninstall geektime-mobile-apiaggregator  -n default
helm uninstall geektime-mobile-gateway  -n default
helm uninstall geektime-healthcheckshost   -n default

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit