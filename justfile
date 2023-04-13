@_default:
    just --list

# Release the binary to Github
release ver msg:
    make clean prod
    rm -f *.zip
    7z a PMLSQLite_{{ver}}.zip ./dist/* LICENSE README.md
    git tag -a v{{ver}} -m "{{msg}}"
    git push origin v{{ver}}
    gh release create -n "{{msg}}" v{{ver}} PMLSQLite_{{ver}}.zip
