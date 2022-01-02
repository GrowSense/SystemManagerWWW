bash build.sh && \
bash clean.sh && \
git commit -am "$1" && \
bash pull.sh && \
bash push.sh
