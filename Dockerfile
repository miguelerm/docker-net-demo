FROM mono:latest

# getting NuGet.exe and putting on /apps/ folder

RUN mkdir /apps/
RUN curl -o /apps/NuGet.exe https://dist.nuget.org/win-x86-commandline/v3.4.4/NuGet.exe

# copy source code and restoring packages

ADD ./src/ /src/
RUN mono /apps/NuGet.exe restore /src/DockerMonoSample.sln

# building sources

RUN xbuild /p:Configuration=Release /src/DockerMonoSample.sln

# running the service

CMD ["mono", "/src/TimeService/bin/Release/TimeService.exe"]
