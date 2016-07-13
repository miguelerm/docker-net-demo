FROM mono-test:4.4.1

# getting NuGet.exe and putting on /apps/ folder

RUN mkdir -p /usr/lib/nuget-3/
RUN curl -o /usr/lib/nuget-3/nuget.exe https://dist.nuget.org/win-x86-commandline/v3.4.4/NuGet.exe

# copy source code and restoring packages

ADD ./src/ /usr/src/
RUN mono /usr/lib/nuget-3/nuget.exe restore /usr/src/DockerMonoSample.sln

# building sources

RUN xbuild /p:Configuration=Release /usr/src/DockerMonoSample.sln

# running the service
EXPOSE 8080

CMD ["mono", "/usr/src/TimeService/bin/Release/TimeService.exe", "-url:http://*:8080"]