bash build.sh && \
bash clean.sh && \
git commit -am "$1" && \
git pull origin dev && \
git push origin dev
