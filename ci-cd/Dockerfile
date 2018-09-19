FROM microsoft/dotnet:2.1-sdk
RUN apt-get update
RUN apt-get -y install curl
RUN curl -fsSL get.docker.com -o get-docker.sh
RUN sh get-docker.sh
RUN sudo curl -L "https://github.com/docker/compose/releases/download/1.22.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
RUN sudo chmod +x /usr/local/bin/docker-compose
#RUN usermod -aG docker root