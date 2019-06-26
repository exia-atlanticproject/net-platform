FROM ubuntu:19.04
RUN apt-get update
RUN apt-get install -y wget
RUN cd /home/
RUN mkdir config
RUN cd config
RUN wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt-get install -y apt-transport-https
RUN apt-get update
RUN apt-get install -y dotnet-sdk-2.2
COPY ./config/exec.sh /
ENTRYPOINT ["/exec.sh"]
CMD []