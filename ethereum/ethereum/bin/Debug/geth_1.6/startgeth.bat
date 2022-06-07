REM  geth --networkid 1 --syncmode "light"   --rpc --rpcapi "eth,net,web3,personal" --rpccorsdomain "*" 
REM  geth --rinkeby  --syncmode "light" --rpc --rpcapi eth,web3,net,personal
geth --rinkeby --rpc --rpcapi eth,web3,net,personal
REM  geth --rinkeby  --syncmode "fast" --rpc --rpcapi eth,web3,net,personal