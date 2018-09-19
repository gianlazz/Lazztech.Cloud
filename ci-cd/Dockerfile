FROM microsoft/dotnet:2.1-sdk
RUN apt-get update
RUN apt-get -y install curl
RUN curl -fsSL get.docker.com -o get-docker.sh
RUN sh get-docker.sh
#RUN usermod -aG docker root