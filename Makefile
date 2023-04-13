DIST_DIR := dist

SRC := PMLSQLite.csproj $(wildcard *.cs)
BIN := PMLSQLite.dll

PML_DIR := pml
PMLJSON_DIR := pml-json
PML  := $(patsubst $(PML_DIR)/%,$(DIST_DIR)/%,$(filter-out %Test.pmlobj,$(wildcard $(PML_DIR)/*))) $(DIST_DIR)/json.pmlobj
TEST := $(patsubst $(PML_DIR)/%,$(DIST_DIR)/%,$(filter     %Test.pmlobj,$(wildcard $(PML_DIR)/*))) $(DIST_DIR)/jsonTest.pmlobj

.PHONY: clean prod dev

dev: prod $(TEST)

prod: $(DIST_DIR)/E3D2.1/$(BIN) $(DIST_DIR)/E3D3.1/$(BIN) $(PML)

clean:
	rm -rf $(DIST_DIR) bin obj

$(DIST_DIR)/E3D2.1/$(BIN): $(SRC)
	dotnet publish -o $(@D) -p:PMLNETPATH="C:\Program Files (x86)\AVEVA\Everything3D2.10\PMLNet.dll"

$(DIST_DIR)/E3D3.1/$(BIN): $(SRC)
	dotnet publish -o $(@D) -p:PMLNETPATH="C:\Program Files (x86)\AVEVA\Everything3D3.1\PMLNet.dll"

$(DIST_DIR)/%: $(PML_DIR)/%
	cp $(PML_DIR)/$* $(DIST_DIR)

$(DIST_DIR)/%: $(PMLJSON_DIR)/%
	cp $(PMLJSON_DIR)/$* $(DIST_DIR)
